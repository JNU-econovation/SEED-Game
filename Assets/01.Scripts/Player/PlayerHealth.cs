using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private HealthBar_ES playerHealthUI;

    private PlayerStun stun;

    private void Awake()
    {
        stun = GetComponent<PlayerStun>();
        currentHealth = maxHealth;
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
        Debug.Log("플레이어 피격 데미지: " + damage);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        AudioManager.Instance.PlayPlayerHit();

        stun.ApplyStun(3f);
        playerHealthUI.SetHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
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
