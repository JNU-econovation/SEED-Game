using System.Collections;
using UnityEngine;

public class PlayerAttack_MK : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weapon;

    private Weapon weaponComponent;
    public bool isAttacking = false;
    private float attackCooldown; // 애니메이션 길이만큼
    
    private PlayerMovement movement;
    
    private void Awake()
    {
        weaponComponent = weapon.GetComponent<Weapon>();
        attackCooldown = weaponComponent.AttackSpeed;
    }

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
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        EnableAttackHitbox();
        // 만약 애니메이션이 
        yield return new WaitForSeconds(attackCooldown);
        DisableAttackHitbox();
    }
    
    private void EnableAttackHitbox()
    {
        weaponComponent.Hitbox.enabled = true;
    }

    private void DisableAttackHitbox()
    {
        weaponComponent.Hitbox.enabled = false;
        isAttacking = false;
    }
}
