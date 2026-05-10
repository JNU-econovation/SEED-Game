using System;
using UnityEngine;

[RequireComponent(typeof(EnemyAI), typeof(EnemyMovement), typeof(EnemyHealth))]
[RequireComponent(typeof(AudioSource), typeof(EnemyStun))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfos infos;

    public static event Action<Enemy> onEnemyDied;
    public EnemyInfos enemyInfos => infos;

    public EnemyMovement EnemyMovement { get; private set; }
    public EnemyAI EnemyAi { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }
    public EnemySpawner spawner { get; set; }

    /// <summary>
    /// EnemySpawner가 Spawn 시 설정합니다.
    /// ObjectPoolManager.ReturnToPool 호출에 필요합니다.
    /// </summary>
    [HideInInspector] public GameObject sourcePrefab;

    private void Awake()
    {
        EnemyMovement = GetComponent<EnemyMovement>();
        EnemyAi       = GetComponent<EnemyAI>();
        EnemyHealth   = GetComponent<EnemyHealth>();
    }

    void OnEnable()
    {
        EnemyHealth.onDeath += Die;

        // 풀에서 재사용될 때 항상 상태 초기화 (사망/스턴/공격 등 모든 상태 대응)
        EnemyHealth.ResetHealth();
        EnemyAi.ResetState();
    }

    void OnDisable()
    {
        EnemyHealth.onDeath -= Die;
    }

    void Die()
    {
        onEnemyDied?.Invoke(this);
    }

    /// <summary>
    /// 사망 애니메이션 종료 이벤트에서 호출됩니다.
    /// Destroy(gameObject) 대신 풀로 반환합니다.
    /// </summary>
    public void ReturnToPool()
    {
        if (sourcePrefab != null)
            ObjectPoolManager.Instance.ReturnToPool(sourcePrefab, gameObject);
        else
            Destroy(gameObject); // 풀 미등록 시 폴백
    }

    // 하위 호환성: 애니메이션 이벤트가 Destroy()를 호출하는 경우를 위해 유지
    public void Destroy() => ReturnToPool();
}
