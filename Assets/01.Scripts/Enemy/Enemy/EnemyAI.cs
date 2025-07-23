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
    private float skillRetryDelay = 2f;
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
            if (currentState == EnemyState.Dead || (enemyInfos != null && GetComponent<EnemyHealth>().IsDead()))
                return;
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
            if (!enemyInfos.isBoss && currentState != EnemyState.Hit && currentState != EnemyState.Dead)
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
        if ((newState == EnemyState.Attack || newState == EnemyState.SkillAttack1 || newState == EnemyState.SkillAttack2) && player != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            dir.y = 0;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
        }
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
            Vector3 toPlayer = (player.position - transform.position).normalized;
            toPlayer.y = 0;
            float dot = Vector3.Dot(transform.forward, toPlayer);
            bool isPlayerInFront = dot > 0.5f;

            if (!isPlayerInFront)
            {
                return EnemyState.Chase;
            }
            
            if (bossSkill != null && bossSkill.IsCooldownOver() && Time.time >= nextSkillTryTime)
            {
                if (bossSkill.CheckSkillChance(0.4f))
                    return EnemyState.SkillAttack1;
                else
                    nextSkillTryTime = Time.time + skillRetryDelay;
            }
            return EnemyState.Attack;
        }
        else if (distance > attackRange && distance < detectionRange)
        {
            if (distance < 10f)
            {
                if (bossSkill != null && bossSkill.IsCooldownOver() && Time.time >= nextSkillTryTime)
                {
                    if (bossSkill.CheckSkillChance(0.5f))
                        return EnemyState.SkillAttack2;
                    else
                        nextSkillTryTime = Time.time + skillRetryDelay; 
                }
            }
            return EnemyState.Chase;
        }
        return EnemyState.Idle;
    }

    public void ResetState()
    {
        currentState = EnemyState.Idle;

        animator.ResetTrigger("Chase");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("SkillAttack1");
        animator.ResetTrigger("SkillAttack2");
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Dead");

        animator.SetTrigger("Idle");

        movement?.Stop();
    }

    public void Die()
    {
        currentState = EnemyState.Dead;

        animator.ResetTrigger("Chase");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("SkillAttack1");
        animator.ResetTrigger("SkillAttack2");
        animator.ResetTrigger("Hit");

        animator.SetTrigger("Dead");

        movement?.Stop();
    }
    public void OnAttackEnd()
    {
        if (currentState == EnemyState.Attack)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            EnemyState postAttackState = DetermineState(distance);

            animator.ResetTrigger("Attack");
            animator.SetTrigger(postAttackState.ToString());
            currentState = postAttackState;
        }
    }
}