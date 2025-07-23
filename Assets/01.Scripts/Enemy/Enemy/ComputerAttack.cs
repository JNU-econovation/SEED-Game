using UnityEngine;

public class ComputerAttack : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    
    private ParticleSystem attackEffect;
    private Animator animator;
    private Weapon weaponComponent;

    [SerializeField] private int attackSoundIndex = 0; 

    private void Awake()
    {
        weaponComponent = weapon.GetComponent<Weapon>();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator가 Enemy 오브젝트에 없습니다!");
        }

        attackEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void EnableAttackHitbox()
    {
        weaponComponent.Hitbox.enabled = true;
    }

    public void DisableAttackHitbox()
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
        AudioManager.Instance.PlayMonsterAttack(attackSoundIndex);
    }
}
