using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfos infos;

    public static event Action<Enemy> OnEnemyDied;
    public EnemyInfos enemyInfos => infos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Invoke(nameof(die), 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void die()
    {
        OnEnemyDied?.Invoke(this);
        Destroy(this.gameObject);
    }
}
