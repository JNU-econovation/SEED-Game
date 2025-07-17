using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private HealthBar_ES playerHealthUI;

    [SerializeField] private Animator animator;

    private ClueBox clueBox;
    private PlayerStun stun;
    public bool enableStun = true;
    private bool isDead = false;


    private void Awake()
    {
        stun = GetComponent<PlayerStun>();
        currentHealth = maxHealth;
        clueBox = GetComponentInChildren<ClueBox>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponentInParent<Weapon>();
        if (weapon == null) return;
        if (weapon.tag != "EnemyWeapon") return;

        float damage = weapon.Damage;
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log("플레이어 피격 데미지: " + damage);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (enableStun)
            stun.ApplyStun(.5f);

        AudioManager.Instance.PlayPlayerHit();

        playerHealthUI.SetHealth(currentHealth);

        if (currentHealth <= 0.0001f)
        {
            currentHealth = 0f; // 확실히 0으로 고정
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // 이미 죽었으면 다시 실행 X

        isDead = true;
        Debug.Log("플레이어 사망");

        animator.SetTrigger("IsDead");

        clueBox.LoseClue();
        
        var moveScript = GetComponent<PlayerMovement>();
        moveScript.isDead = true;

        moveScript.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }


    private void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    void Start()
    {
        playerHealthUI.Init(maxHealth);
    }

    void Update()
    {
        
    }
}
