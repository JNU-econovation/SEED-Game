using System;
using UnityEngine;

[RequireComponent(typeof(EnemyAI), typeof(EnemyMovement), typeof(EnemyHealth))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfos infos;

    public static event Action<Enemy> onEnemyDied;
    public EnemyInfos enemyInfos => infos;

    public EnemyMovement EnemyMovement {get; private set; }
    public EnemyAI EnemyAi {get; private set; }
    public EnemyHealth EnemyHealth {get; private set; }
    // public EnemyAttack EnemyAttack {get; private set; }
    
    private void Awake()
    {
        EnemyMovement = GetComponent<EnemyMovement>();
        EnemyAi = GetComponent<EnemyAI>();
        EnemyHealth = GetComponent<EnemyHealth>();
        // EnemyAttack = GetComponent<EnemyAttack>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Invoke(nameof(Die), 15f);
        EnemyHealth.onDeath += Die;
    }

    void OnDisable()
    {
        EnemyHealth.onDeath -= Die;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        onEnemyDied?.Invoke(this);
        Destroy(this.gameObject);
    }
}
