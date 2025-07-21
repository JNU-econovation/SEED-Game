using UnityEngine;
using UnityEngine.AI;

public class EnemyStun : MonoBehaviour
{
    public bool isStunned = false;
    private float stunTimer = 1.5f;

    [SerializeField] private MonoBehaviour[] componentsToDisableDuringStun;
    [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private EnemyInfos enemyInfos;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Awake()
    {
        var enemy = GetComponent<Enemy>();
        if (enemy != null)
            enemyInfos = enemy.enemyInfos;
    }
    void Update()
    {
        if (isStunned)
        {
            Debug.Log($"스턴 유지 중... 남은 시간: {stunTimer:F2}");
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                EndStun();
            }
        }
    }

    public void ApplyStun(float duration)
    {
        if (enemyInfos.isBoss) return;
        Debug.Log($"ApplyStun 호출됨 - duration: {duration}, 현재 isStunned: {isStunned}, 현재 stunTimer: {stunTimer}");
    
        // 이미 스턴 중이고, 지금 들어온 duration이 더 짧으면 무시
        if (isStunned && stunTimer > duration)
        {
            Debug.Log($"무시됨: 현재 스턴 남은 시간 {stunTimer:F2} > 새 요청 {duration}");
            return;
        }

        isStunned = true;
        stunTimer = duration;
        Debug.Log($"스턴 타이머 설정완료: {stunTimer}");

        foreach (var comp in componentsToDisableDuringStun)
        {
            if (comp != null) comp.enabled = false;
        }

        if (agent != null)
            agent.isStopped = true;  // NavMeshAgent 멈추기

        Debug.Log("몬스터 스턴 적용 + 애니메이션");
    }

    public void ApplyStun()
    {
        ApplyStun(stunTimer);
    }

    void EndStun()
    {
        isStunned = false;

        foreach (var comp in componentsToDisableDuringStun)
        {
            if (comp != null) comp.enabled = true;
        }

        if (agent != null)
            agent.isStopped = false;  // NavMeshAgent 다시 움직이게

        Debug.Log("몬스터 스턴 해제");
    }

    public bool IsStunned() => isStunned;
}
