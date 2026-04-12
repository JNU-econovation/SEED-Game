# 리팩토링 현황 및 TODO

## 분석 일자: 2026-04-11

---

## 변경된 파일

| 파일 | 변경 내용 |
|------|-----------|
| `Assets/01.Scripts/ObjectPoolManager.cs` | 신규 추가 — 프리팹 기반 오브젝트 풀 싱글톤 |
| `Assets/01.Scripts/Enemy/Enemy/Enemy.cs` | `Destroy()` → `ReturnToPool()`, OnEnable에서 상태 초기화 |
| `Assets/01.Scripts/Enemy/Enemy/EnemySpawner.cs` | `Instantiate` → `ObjectPoolManager.Spawn` |
| `Assets/01.Scripts/Player/PlayerAttack_MK.cs` | 무기 모델 `Instantiate/Destroy` → 풀 사용, `SpawnWeaponModel()` 공용 헬퍼 추출 |
| `Assets/01.Scripts/Player/PlayerMovement.cs` | 점프 `AddForce` → `pendingJump` 플래그로 FixedUpdate에서 처리, `sqrMagnitude` 최적화 |

---

## 프로파일러 측정 결과 (에디터 플레이 모드, 285프레임)

| 항목 | 수치 |
|------|------|
| 평균 프레임타임 | 73.8ms (14 FPS) |
| PlayerLoop (게임 로직) | **~0ms** (측정 불가 수준) |
| EditorLoop (에디터 오버헤드) | **~98ms** (전체의 99%) |
| 30fps 미달 프레임 비율 | 74% |

> **결론**: 14 FPS는 씬뷰·인스펙터 렌더링 등 Unity 에디터 자체 오버헤드입니다.
> 게임 로직의 CPU 점유는 거의 0에 가까우며, 스탠드얼론 빌드에서는 정상 프레임이 예상됩니다.

---

## 발견된 버그

### ✅ 수정 완료

**`ObjectPoolManager.Instance` NullReferenceException**
- 위치: `EnemySpawner.SpawnEnemy()` (L42), `PlayerAttack_MK.SpawnWeaponModel()`
- 원인: 씬에 `ObjectPoolManager` GameObject가 없어 `Instance`가 `null`
- 수정: `ObjectPoolManager`를 lazy 싱글톤으로 변경 — 첫 접근 시 자동으로 `new GameObject`를 생성해 `DontDestroyOnLoad`로 등록

```csharp
// 수정 전
public static ObjectPoolManager Instance { get; private set; }

// 수정 후
public static ObjectPoolManager Instance {
    get {
        if (_instance == null) {
            var go = new GameObject("ObjectPoolManager");
            _instance = go.AddComponent<ObjectPoolManager>();
            DontDestroyOnLoad(go);
        }
        return _instance;
    }
}
```

---

### ⚠️ 미해결 (기존 문제 — 리팩토링 이전부터 존재)

**Kinematic Rigidbody에 `linearVelocity` 직접 세팅**
- 위치: `PlayerMovement.cs` `FixedUpdate()` 전체 (L187, L194, L204, L208, L213)
- 경고: `Setting linear velocity of a kinematic body is not supported.`
- 원인: Player Rigidbody의 `isKinematic`이 true인 상태에서 `rb.linearVelocity`를 직접 설정
- 영향: 이동/점프가 의도한 대로 동작하지 않을 가능성 있음

---

## TODO

### 긴급

- [ ] **Player Rigidbody `isKinematic` 확인**
  Inspector → Player → Rigidbody → `Is Kinematic` 체크 여부 확인
  - Kinematic이 `true`면: `rb.linearVelocity` 방식 전체를 `rb.MovePosition`으로 교체
  - Kinematic이 `false`면: 경고의 원인을 다시 조사

- [ ] **`ObjectPoolManager` 씬 프리팹화 (선택)**
  현재 lazy 싱글톤으로 자동 생성되지만, 씬에 명시적으로 배치해두면 풀 사전 워밍업(`Preload`) 기능 추가가 용이함

### 일반

- [ ] **풀 반환 누락 케이스 점검**
  `PlayerAttack_MK.ResetWeapon()`에서 `spawnedModel`이 null인 상태로 공격이 중단될 때 반환이 호출되는지 확인

- [ ] **EnemyHealth.ResetHealth() / EnemyAI.ResetState() 검증**
  풀 재사용 시 `OnEnable`에서 호출되는 리셋이 모든 상태(NavMesh agent, 애니메이터, 히트박스 등)를 완전히 초기화하는지 플레이 테스트로 확인

- [ ] **스탠드얼론 빌드 FPS 측정**
  에디터에서의 14 FPS는 에디터 오버헤드 탓이므로, Development Build로 빌드 후 실제 FPS를 측정해 목표 프레임(60fps 또는 30fps)과 비교

- [ ] **점프 느낌 검증**
  `pendingJump` 방식(`linearVelocity` 직접 세팅)이 기존 `AddForce` 대비 점프감 차이가 없는지 QA

---

## 참고: ObjectPoolManager 사용법

```csharp
// 스폰 (Instantiate 대신)
GameObject obj = ObjectPoolManager.Instance.Spawn(prefab, position, rotation);

// 반환 (Destroy 대신)
ObjectPoolManager.Instance.ReturnToPool(prefab, obj);
```

> 반환 시 원본 `prefab` 레퍼런스가 필요합니다.
> `Enemy.sourcePrefab`, `PlayerAttack_MK.spawnedModelPrefab`에 스폰 시점에 저장하는 방식으로 구현되어 있습니다.
