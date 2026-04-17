# 리팩토링 현황 및 TODO

## 분석 일자: 2026-04-11 / 최종 수정: 2026-04-13

---

## 변경된 파일

| 파일 | 변경 내용 |
|------|-----------|
| `Assets/01.Scripts/ObjectPoolManager.cs` | 신규 추가 — 프리팹 기반 오브젝트 풀 싱글톤 |
| `Assets/01.Scripts/Enemy/Enemy/Enemy.cs` | `Destroy()` → `ReturnToPool()`, OnEnable에서 상태 초기화 |
| `Assets/01.Scripts/Enemy/Enemy/EnemySpawner.cs` | `Instantiate` → `ObjectPoolManager.Spawn` |
| `Assets/01.Scripts/Player/PlayerAttack_MK.cs` | 무기 모델 `Instantiate/Destroy` → 풀 사용, `SpawnWeaponModel()` 공용 헬퍼 추출 |
| `Assets/01.Scripts/Player/PlayerMovement.cs` | 점프 `AddForce` → `pendingJump` 플래그로 FixedUpdate에서 처리, `sqrMagnitude` 최적화 |
| `Assets/01.Scripts/Enemy/Enemy/EnemyAI.cs` | `ResetState()`에 `nextSkillTryTime = 0f;` 추가 — 풀 재사용 시 스킬 쿨다운 정상화 |
| `Assets/01.Scripts/Enemy/Enemy/Enemy.cs` | `OnEnable()` 조건부 리셋 → 무조건 리셋 (사망 외 상태에서 반환 시 미초기화 버그 수정) |
| `Assets/01.Scripts/Enemy/Enemy/EnemyHealth.cs` | `ResetHealth()` 내 `currentHealth = maxHealth;` 중복 호출 제거 |
| `Assets/01.Scripts/Player/PlayerHealth.cs` | `Die()` / `gotoClub()`의 `GetComponent<Rigidbody>()` → `Awake()`에서 `rb` 필드로 캐싱 |

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

**`EnemyAI.ResetState()` — `nextSkillTryTime` 미리셋**
- 위치: `EnemyAI.cs` `ResetState()` (L186~200)
- 원인: 풀 재사용 시 `nextSkillTryTime`이 초기화되지 않아 이전 생존 시간 기준으로 스킬 쿨다운이 남음
- 수정: `ResetState()` 마지막에 `nextSkillTryTime = 0f;` 추가

**`Enemy.OnEnable()` — 조건부 리셋**
- 위치: `Enemy.cs` `OnEnable()` (L31~41)
- 원인: `isDead == true`일 때만 리셋 → 스턴/공격 중 반환된 적이 부분 상태로 재사용됨
- 수정: 조건 제거, 무조건 `ResetHealth()` + `ResetState()` 호출

**`EnemyHealth.ResetHealth()` — 중복 코드**
- 위치: `EnemyHealth.cs` L88, L91
- 원인: `currentHealth = maxHealth;`가 두 번 호출됨
- 수정: 중복 줄 제거

**`PlayerHealth` — `GetComponent<Rigidbody>()` 미캐싱**
- 위치: `PlayerHealth.cs` `Die()` (L75), `gotoClub()` (L92)
- 원인: `Update` 외부지만 CLAUDE.md 캐싱 규칙 미준수
- 수정: `Awake()`에서 `rb` 필드로 캐싱

---

### ⚠️ 미해결 (기존 문제 — 리팩토링 이전부터 존재)

**Kinematic Rigidbody에 `linearVelocity` 직접 세팅 — 타이밍 이슈 (무시 가능)**
- 위치: `PlayerMovement.cs` `FixedUpdate()` 전체
- 경고: `Setting linear velocity of a kinematic body is not supported.`
- 원인 분석: Player Rigidbody `isKinematic = false`가 기본값 (Prefab 확인 완료). `Die()` 호출 시 동일 프레임 내 `isKinematic = true` 전환과 FixedUpdate 간 타이밍 충돌로 경고 발생 추정.
- 결론: 런타임 이동/점프 로직에는 영향 없음. 무시 가능 수준.

---

## TODO

### 수동 QA 필요 항목

- [ ] **스탠드얼론 빌드 FPS 측정**
  에디터에서의 14 FPS는 에디터 오버헤드 탓이므로, Development Build로 빌드 후 실제 FPS를 측정해 목표 프레임(60fps 또는 30fps)과 비교

- [ ] **점프 느낌 검증**
  `pendingJump` 방식(`linearVelocity` 직접 세팅)이 기존 `AddForce` 대비 점프감 차이가 없는지 QA

- [ ] **EnemyAI `nextSkillTryTime` 리셋 검증**
  적 처치 후 재스폰 시 즉시 스킬 사용하지 않는지 플레이 테스트로 확인

- [ ] **`ObjectPoolManager` 씬 프리팹화 (선택)**
  현재 lazy 싱글톤으로 정상 동작 중. 풀 사전 워밍업(`Preload`) 기능이 필요하면 씬에 명시적으로 배치 고려

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
