# 🌱 SEED - 내러티브 액션 퍼즐 게임

> 동아리 방에서 시작되는 의문, 단서를 모아 진실을 밝혀라

**SEED**는 Unity 6 기반의 3D 액션 어드벤처 게임입니다.
플레이어는 의문스러운 동아리 방을 탐험하며 적과 싸우고, 적에게서 단서를 수집·조합해 사건의 전말을 파헤칩니다.
퍼즐 잠금 해제, 보스 전투, 단서 합성 시스템이 어우러진 긴장감 있는 플레이 경험을 제공합니다.

## 👥 팀 정보
- **개발 기간**: 2025.04 ~ 2025.07
- **팀 구성**: 박민규, 김현규, 이은상

## 📌 주요 기능

### ⚔️ 전투 시스템
- **5가지 무기**: 맨손, 연필(투척), 노트북(스매시), 마우스(채찍), 빔프로젝터(원거리) — 무기마다 고유 공격 모션
- **플레이어 행동**: WASD 이동 + Shift 달리기 + Space 점프 + Ctrl 구르기(회피)
- **피격 반응**: 피격 시 0.5초 스턴, 체력 소진 시 동아리 방 리스폰 및 보유 단서 일부 손실

### 🤖 적 AI 시스템
- **상태 기반 AI**: Idle → Chase → Attack / SkillAttack1 / SkillAttack2 → Hit → Dead 전환
- **4종 일반 적**: 노트북, 의자, 컴퓨터, 라커 — 각기 다른 공격 패턴
- **NavMeshAgent 이동**: 장애물 회피 경로 탐색, 감지 범위 내 플레이어 추적

### 🗂️ 단서 수집 & 합성 시스템
- **단서 획득**: 적 처치 시 단서 조각(1~4번) 드롭
- **4조각 합성**: 같은 단서의 조각 4개를 모으면 완성 단서로 자동 병합
- **4종 단서**: 완성 단서 4개가 핵심 퍼즐 및 보스 조우 조건과 연결
- **단서 UI**: Q 키로 단서 박스 토글, 단서 목록 / 상세 설명 패널 제공

### 🔐 퍼즐 시스템
- **키패드 잠금 해제**: 동아리 방 입구의 암호 키패드 — 비밀번호 입력 시 문 개방
- **단서 노트**: 특수 단서(computerHint, CardKey) 습득 시 힌트 노트 UI 표시
- **보스 중간 퍼즐**: 보스 HP 임계값 도달 시 순서 맞추기(SequencePuzzle) / 스매시(SmashPuzzle) 등장

### 👾 보스 전투
- **보스 등장**: 특정 조건 달성 시 BossEntranceTrigger가 Timeline 연출 재생 후 전투 개시
- **2페이즈**: 일반 공격 패턴 → HP 임계값에서 퍼즐 페이즈 전환
- **레이저 스킬**: 보스 고유 레이저 공격 (쿨타임 + 확률 기반 발동)
- **사망 연출**: BossDeathTimelineTrigger로 엔딩 컷신 재생

### 🔊 오디오 시스템
- **중앙 관리**: AudioManager 싱글톤이 BGM / SFX를 전역 제어
- **BGM 3종**: 기본 탐험 테마, 보스 전투 테마, 엔딩
- **SFX**: 플레이어(걷기·달리기·점프·구르기·공격·피격), 적(공격·피격·사망), 보스 스킬, UI 효과음

## 🛠 기술 스택

### 엔진 & 렌더링
- **Unity 6** (6000.1.3f1)
- **Universal Render Pipeline (URP)** 17.1.0
- **Shader Graph** 17.1.0
- **Visual Effect Graph** 17.1.0
- **Post Processing** 3.4.0

### 게임플레이 시스템
- **Input System** 1.14.0 — New Input System 기반 키 입력
- **AI Navigation** 2.0.8 — NavMeshAgent 경로 탐색
- **Cinemachine** 3.1.4 — 인게임 카메라 제어
- **Timeline** 1.8.8 — 보스 등장·사망 컷신 연출

### 아키텍처
- **Singleton Manager**: AudioManager, ObjectPoolManager, BossManager
- **State Machine (Enum 기반)**: EnemyAI 상태 전환
- **ScriptableObject Data**: EnemyInfos, AttackInfos, ClueInfos — 데이터 드리븐 설계
- **Object Pooling**: `Dictionary<GameObject, Queue<GameObject>>` 기반 커스텀 풀

## 🏗 프로젝트 구조

```
Assets/
├── 01.Scripts/
│   ├── Player/             # 이동, 공격, 체력, 스태미나, 스턴, 힐, 무기 전환
│   ├── Enemy/
│   │   ├── Enemy/          # AI, 이동, 공격, 체력, 스폰, 스턴
│   │   └── Boss/
│   │       └── 01.Boss/    # 보스 관리, 스킬, 레이저, 퍼즐
│   ├── ClueBox/            # 단서 수집, 합성, 카드키, UI 패널
│   ├── UI/                 # 메뉴, 키패드, 텍스트, 인터랙션 트리거
│   ├── AudioManager.cs
│   ├── ObjectPoolManager.cs
│   ├── SigninManager.cs
│   └── CloseComputer.cs
├── 02.Prefabs/             # 플레이어, 적, UI, 이펙트 프리팹
├── 03.Animations/          # 애니메이터 컨트롤러 및 클립
├── 04.ScriptableObjects/
│   ├── Player/             # AttackInfos, weapon1~5
│   ├── Enemy/              # 4종 적 + 보스 EnemyInfos / AttackInfos
│   └── ClueBox/            # 완성 단서 4종 + 조각 16종 + CardKey
└── 08.Assets/              # 맵, 사운드, 텍스처, 머티리얼
```

## 🎯 핵심 설계 원칙

### 1. Singleton 매니저 패턴
전역 시스템(오디오, 풀링, 보스)을 Singleton으로 통일해 씬 간 상태를 유지하고 어디서든 단일 진입점으로 접근합니다.
ObjectPoolManager는 씬에 배치하지 않아도 최초 접근 시 자동으로 생성되는 Lazy Singleton으로 구현했습니다.

```csharp
// ObjectPoolManager — 씬 없이도 자동 생성되는 Lazy Singleton
public static ObjectPoolManager Instance
{
    get
    {
        if (_instance == null)
        {
            var go = new GameObject("ObjectPoolManager");
            _instance = go.AddComponent<ObjectPoolManager>();
            DontDestroyOnLoad(go);
        }
        return _instance;
    }
}
```

### 2. 열거형 기반 상태 머신 (Enemy AI)
EnemyAI는 열거형(Enum) 기반 상태 머신으로 동작합니다. 상태 전환 시 이전 Trigger를 Reset한 뒤 새 Trigger를 Set해 Animator와 일관성을 보장합니다.

```csharp
public enum EnemyState { Idle, Chase, Attack, SkillAttack1, SkillAttack2, Hit, Dead }

private void HandleState(float distance)
{
    EnemyState newState = DetermineState(distance);
    if (currentState == newState) return;

    animator.ResetTrigger(currentState.ToString());
    animator.SetTrigger(newState.ToString());
    currentState = newState;

    if (currentState == EnemyState.SkillAttack1) bossSkill?.TryCastSkill1();
    if (currentState == EnemyState.SkillAttack2) bossSkill?.TryCastSkill2();
}
```

### 3. 오브젝트 풀링
투사체·스킬 이펙트처럼 자주 생성·소멸하는 오브젝트에는 `Instantiate / Destroy` 대신 커스텀 풀을 사용해 GC 압력을 제거합니다.

```csharp
// ❌ Bad: 매 호출마다 GC Alloc 유발
Instantiate(bulletPrefab, spawnPos, rotation);
Destroy(bullet, 3f);

// ✅ Good: 풀에서 꺼내고 반납
GameObject bullet = ObjectPoolManager.Instance.Spawn(bulletPrefab, spawnPos, rotation);
ObjectPoolManager.Instance.ReturnToPool(bulletPrefab, bullet);
```

```csharp
// 내부 구현 — 프리팹별 큐로 관리
private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();

public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
{
    if (!pools.TryGetValue(prefab, out var queue))
    {
        queue = new Queue<GameObject>();
        pools[prefab] = queue;
    }
    var obj = queue.Count > 0 ? queue.Dequeue() : Instantiate(prefab);
    obj.transform.SetPositionAndRotation(position, rotation);
    obj.SetActive(true);
    return obj;
}
```

### 4. ScriptableObject 데이터 드리븐 설계
적의 스탯, 공격 정보, 단서 메타데이터를 ScriptableObject로 분리해 코드 수정 없이 에디터에서 밸런스 조정과 콘텐츠 추가가 가능합니다.

```csharp
[CreateAssetMenu(fileName = "EnemyInfos", menuName = "ScriptableObject/EnemyInfos")]
public class EnemyInfos : ScriptableObject
{
    public string enemyName;
    public float maxHealth;
    public float detectionRange;
    public bool isBoss;
    public AttackInfos attackInfo;
}

[CreateAssetMenu(fileName = "ClueInfos", menuName = "ScriptableObject/ClueInfos")]
public class ClueInfos : ScriptableObject
{
    public string clueName;
    public string description;
    public Sprite clueImage;
    public int clueIndex;          // 0: 완성 단서, 1~4: 조각 단서
    public ClueInfos completeClue; // 이 조각이 속한 완성 단서 참조
}
```

### 5. 컴포넌트 캐싱 원칙
`Update()` / `FixedUpdate()` 내부에서의 `GetComponent`, `Find` 호출을 금지하고, 모든 컴포넌트 참조는 `Awake()` / `Start()`에서 캐싱합니다.
벡터 크기 비교 시 `magnitude` 대신 `sqrMagnitude`를 사용해 불필요한 제곱근 연산을 제거합니다.

```csharp
// ❌ Bad: 매 프레임마다 컴포넌트 탐색
void Update() {
    GetComponent<Rigidbody>().AddForce(Vector3.up);
}

// ✅ Good: 초기화 시 1회 캐싱
private Rigidbody rb;
void Start() { rb = GetComponent<Rigidbody>(); }

void Update() {
    // sqrMagnitude로 sqrt 연산 제거
    if (moveDir.sqrMagnitude >= 0.01f)
        rb.linearVelocity = moveVelocity;
}
```

## 🚀 실행 방법

### 필수 환경
- **Unity**: 6000.1.3f1 
- **Render Pipeline**: URP 17.1.0 (Package Manager에서 자동 복원)
- **플랫폼**: Windows (빌드 타겟 `PC, Mac & Linux Standalone`)

### 프로젝트 열기
```
1. Unity Hub 실행 → Open → 프로젝트 루트 폴더(SEED/) 선택
2. Unity 버전 불일치 경고 시 Continue (6000.1.3f1 권장)
3. 패키지 임포트 완료까지 대기 (최초 1회, 수 분 소요)
4. Assets/06.Scenes/ 에서 시작 씬 열기
```

### 플레이 전 체크리스트
- **NavMesh 베이크**: Window → AI → Navigation → Bake
- `AudioManager`, `ObjectPoolManager` 씬 내 GameObject 배치 확인
- `BossManager` 씬 배치 및 BossEnemyInfos ScriptableObject 슬롯 할당 확인
- URP Asset이 Project Settings → Graphics에 올바르게 지정됐는지 확인

### 인게임 실행
- Unity Editor 상단 ▶ **Play** 버튼으로 즉시 플레이 테스트 가능

### 빌드 파일 링크
```
https://kimpro4214.itch.io/seed
```

## 🎮 조작법

| 입력 | 동작 |
|------|------|
| WASD | 이동 (카메라 방향 기준) |
| Shift | 달리기 |
| Space | 점프 |
| Ctrl | 구르기 (회피) |
| LMB | 무기 공격 |
| 1~5 | 무기 전환 |
| Q | 단서 박스 토글 |
| ESC | 인게임 메뉴 토글 |
| E | 오브젝트 상호작용 |

## 🔧 주요 ScriptableObject 목록

| 경로 | 내용 |
|------|------|
| `04.ScriptableObjects/Player/weapon1~5.asset` | 무기별 공격 정보 |
| `04.ScriptableObjects/Enemy/*/EnemyInfos.asset` | 적별 스탯 (노트북·의자·컴퓨터·라커·보스) |
| `04.ScriptableObjects/Enemy/*/AttackInfos.asset` | 적별 공격 정보 |
| `04.ScriptableObjects/ClueBox/Clue1~4.asset` | 완성 단서 4종 |
| `04.ScriptableObjects/ClueBox/Clue*_*.asset` | 조각 단서 16종 (4조각 × 4단서) |
| `04.ScriptableObjects/ClueBox/CardKey.asset` | 카드키 단서 |
| `04.ScriptableObjects/ClueBox/computerHint.asset` | 컴퓨터 힌트 단서 |
