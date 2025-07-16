using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyInfos infos;
    private float maxHealth => infos.maxHealth;
    private float currentHealth { get; set; }
    private bool isDead { get; set; } = false;
    private EnemyStun stun;
    private EnemyAI enemyAI;
    public event Action onDeath;
    private bool puzzleTriggered = false;
    private BossPuzzleManager puzzleManager;

    private void Awake()
    {
        infos = GetComponent<Enemy>().enemyInfos;
        stun = GetComponent<EnemyStun>();
        enemyAI = GetComponent<EnemyAI>();
        puzzleManager = GetComponent<BossPuzzleManager>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void OnHitboxTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponent<Weapon>();
        if (weapon == null) return;
        if (weapon.tag != "PlayerWeapon") return;

        float attackDamage = weapon.Damage;
        Debug.Log("attackDamage: " + attackDamage);
        TakeDamage(attackDamage);

        EnemyHealthUI enemyHealthUI = GetComponentInChildren<EnemyHealthUI>();
        enemyHealthUI.TakeDamage(attackDamage);
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        stun.ApplyStun(1f);

        // ✅ AudioManager로 피격 사운드 재생
        AudioManager.Instance.PlayMonsterHit();

        // 퍼즐 조건
        if (puzzleManager != null && !puzzleTriggered && currentHealth <= maxHealth * 0.2f)
        {
            puzzleTriggered = true;
            puzzleManager.StartPuzzle();
        }
        
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        onDeath?.Invoke();
        isDead = true;
        enemyAI.Die();
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }
}
