using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    SkillAttack1,
    SkillAttack2,
    Hit,
    Dead
}

public class EnemyAI : MonoBehaviour
{
    private EnemyState currentState = EnemyState.Idle;
    private EnemyInfos enemyInfos;
    private Transform player;
    private EnemyMovement movement;
    private Animator animator;
    private EnemyStun stun;
    private BossSkill bossSkill;
    private float skillRetryDelay = 2f;  // 실패 후 재시도 지연 시간
    private float nextSkillTryTime = 0f;



    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
        enemyInfos = GetComponent<Enemy>().enemyInfos;
        stun = GetComponent<EnemyStun>();
        bossSkill = GetComponent<BossSkill>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator.SetTrigger("Idle");
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        HandleState(distance);
    }

    private void HandleState(float distance)
    {

        if (currentState == EnemyState.Dead)
        {
            animator.SetTrigger("Dead");
            return;
        }

        EnemyState newState = DetermineState(distance);

        if (currentState == EnemyState.SkillAttack1 || currentState == EnemyState.SkillAttack2)
        {
            if (bossSkill != null && bossSkill.IsSkillInProgress())
                return;

            // 스킬 끝난 뒤 새 상태 재결정
            EnemyState postSkillState = DetermineState(distance);

            if (postSkillState == EnemyState.SkillAttack1 || postSkillState == EnemyState.SkillAttack2)
            {
                // 다시 스킬 조건이 맞으면 계속 스킬 → 여기서 확률 및 쿨타임 다시 체크됨
                animator.ResetTrigger(currentState.ToString());
                animator.SetTrigger(postSkillState.ToString());
                currentState = postSkillState;
            }
            else
            {
                // 스킬 조건 불충분 → 일반 Attack or Chase
                animator.ResetTrigger(currentState.ToString());
                animator.SetTrigger(postSkillState.ToString());
                currentState = postSkillState;
            }
            return;
        }


        if (player == null) return;

        if (stun != null && stun.isStunned)
        {
            if (currentState != EnemyState.Hit && currentState != EnemyState.Dead)
            {
                animator.ResetTrigger(currentState.ToString());
                animator.SetTrigger(EnemyState.Hit.ToString());
                currentState = EnemyState.Hit;

                movement.Stop();
            }
            return; // 스턴 중에는 상태머신 정지
        }

        // Chase일 때는 계속해서 Move를 호출해야함
        if (currentState == EnemyState.Chase)
            movement.Move(player.position);
        if (currentState == newState && currentState != EnemyState.Dead) return;

        animator.ResetTrigger(currentState.ToString());
        animator.SetTrigger(newState.ToString());
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
            case EnemyState.Hit:
            case EnemyState.Dead:
            case EnemyState.Attack:
                movement.Stop();
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.SkillAttack1:
                movement.Stop();
                bossSkill?.TryCastSkill1();
                break;
            case EnemyState.SkillAttack2:
                movement.Stop();
                bossSkill?.TryCastSkill2();
                break;
        }
    }

    private EnemyState DetermineState(float distance)
    {
        if (currentState == EnemyState.Dead) return EnemyState.Dead;

        float attackRange = enemyInfos.attackInfo.attackRange;
        float detectionRange = enemyInfos.detectionRange;

        if (distance < attackRange)
        {
            if (bossSkill != null && bossSkill.IsCooldownOver() && Time.time >= nextSkillTryTime)
            {
                if (bossSkill.CheckSkillChance(0.3f))
                    return EnemyState.SkillAttack1;
                else
                    nextSkillTryTime = Time.time + skillRetryDelay; // 실패 시 재시도 지연
            }
            return EnemyState.Attack;
        }
        else if (distance < detectionRange)
        {
            if (bossSkill != null && bossSkill.IsCooldownOver() && Time.time >= nextSkillTryTime)
            {
                if (bossSkill.CheckSkillChance(0.5f))
                    return EnemyState.SkillAttack2;
                else
                    nextSkillTryTime = Time.time + skillRetryDelay; // 실패 시 재시도 지연
            }
            return EnemyState.Chase;
        }
        return EnemyState.Idle;
    }

    public void Die()
    {
        currentState = EnemyState.Dead;
    }
}