using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackCollider;

    public bool isAttacking = false;
    private float attackCooldown = 1.0f; // 애니메이션 길이만큼
    private float attackTimer = 0f;

    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // 구르기 중엔 공격 허용 X
        if (movement != null && movement.isRolling) return;
        // 공격 입력
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            attackTimer = attackCooldown;
        }

        // 공격 시간 경과 체크
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }
    }

    public void EnableAttackHitbox()
    {
        attackCollider.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        attackCollider.SetActive(false);
    }

}
