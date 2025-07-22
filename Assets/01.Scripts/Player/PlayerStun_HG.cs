using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    public bool isStunned = false;
    private float stunTimer = 0f;

    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Animator animator;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                if (movement != null) movement.enabled = true;
            }
        }
    }

    public void ApplyStun(float duration)
    {
        if (isStunned) return;

        isStunned = true;
        stunTimer = duration;
        movement.enabled = false;

        if (animator != null)
        {
            animator.SetTrigger("Hit"); // ← 애니메이션 재생
        }

        Debug.Log("플레이어 스턴 + 피격 애니메이션 실행");
    }

    public bool IsStunned() => isStunned;
}

