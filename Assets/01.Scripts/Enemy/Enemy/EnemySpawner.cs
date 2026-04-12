using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnRate = 5f;

    private int currentEnemies = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private void OnEnable()
    {
        Enemy.onEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        Enemy.onEnemyDied -= OnEnemyDied;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies)
                SpawnEnemy();

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnEnemy()
    {
        // Instantiate 대신 풀에서 꺼냄 → GC 할당 없음
        GameObject enemyObj = ObjectPoolManager.Instance.Spawn(
            enemyPrefab,
            GetRandomPosition(),
            Quaternion.identity
        );

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.sourcePrefab = enemyPrefab; // 반환 시 사용할 원본 프리팹 참조 전달
        enemy.spawner = this;
        currentEnemies++;
    }

    private Vector3 GetRandomPosition()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, bounds.center.y, z);
    }

    private void OnEnemyDied(Enemy deadEnemy)
    {
        if (deadEnemy.spawner == this)
            currentEnemies--;
    }
}
