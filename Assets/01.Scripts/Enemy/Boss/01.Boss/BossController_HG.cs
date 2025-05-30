using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (agent.isOnNavMesh)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < 5f)
            {
                agent.SetDestination(player.position);
            }

            // 여기서 속도에 따라 애니메이션 전환
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }
}
