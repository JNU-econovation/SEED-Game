using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    
    private ParticleSystem attackEffect;
    private Animator animator;
    private Weapon weaponComponent;

    [SerializeField] private int attackSoundIndex = 0; // AudioManager에서 몬스터/보스 공격 사운드 index 지정

    private void Awake()
    {
        weaponComponent = weapon.GetComponent<Weapon>();

        // ✅ Enemy 본체에서 Animator 가져오기
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator가 Enemy 오브젝트에 없습니다!");
        }

        attackEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // ✅ Animator가 없으면 검사하지 않도록 방어
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            EnableAttackHitbox();
            return;
        }
        DisableAttackHitbox();
    }

    private void EnableAttackHitbox()
    {
        weaponComponent.Hitbox.enabled = true;
    }

    private void DisableAttackHitbox()
    {
        weaponComponent.Hitbox.enabled = false;
    }

    public void AttackEffect()
    {
        if (attackEffect != null)
        {
            attackEffect.Play();
        }
    }

    public void AttackSound()
    {
        // ✅ AudioManager로 공격 사운드 재생
        AudioManager.Instance.PlayMonsterAttack(attackSoundIndex);
    }
}
