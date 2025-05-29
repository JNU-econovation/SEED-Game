using UnityEngine;
using UnityEngine.AI;

// NavMeshAgent가 없으면 자동으로 붙여줌
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Vector3 targetPosition)
    {
        if (agent.enabled)
        {
            agent.SetDestination(targetPosition);
        }
    }

    public void Stop()
    {
        if (agent.enabled)
        {
            agent.ResetPath();
        }
    }
}
