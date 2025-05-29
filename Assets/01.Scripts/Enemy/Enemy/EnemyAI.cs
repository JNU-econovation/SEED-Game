using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Hit,
    Dead
}

// EnemyMovement가 없으면 자동으로 붙여줌
[RequireComponent(typeof(EnemyMovement))]
public class EnemyAI : MonoBehaviour
{
    public EnemyState currentState = EnemyState.Idle;

    private EnemyInfos enemyInfos;
    private Transform player;
    private EnemyMovement movement;
    private Animator animator;

    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
        enemyInfos = GetComponent<Enemy>().enemyInfos;
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
        EnemyState newState = DetermineState(distance);

        if (player == null) return;
        // Chase일 때는 계속해서 Move를 호출해야함
        if (currentState == EnemyState.Chase)
            movement.Move(player.position);
        if (currentState == newState) return;

        animator.ResetTrigger(currentState.ToString());
        animator.SetTrigger(newState.ToString());
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
            case EnemyState.Attack:
            case EnemyState.Hit:
            case EnemyState.Dead:
                movement.Stop();
                break;
            case EnemyState.Chase:
                break;
        }
    }

    private EnemyState DetermineState(float distance)
    {
        if (distance < enemyInfos.attackInfo.attackRange) return EnemyState.Attack;
        if (distance < enemyInfos.detectionRange) return EnemyState.Chase;
        return EnemyState.Idle;
    }
}