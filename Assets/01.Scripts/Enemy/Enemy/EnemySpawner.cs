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

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void OnEnable()
    {
        Enemy.onEnemyDied += tmp;
    }

    private void OnDisable()
    {
        Enemy.onEnemyDied -= tmp;
    }

    private void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, randomPosition(), Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.spawner = this;
        currentEnemies++;
    }

    private Vector3 randomPosition()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.center.y;
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    void tmp(Enemy deadEnemy)
    {
        if (deadEnemy.spawner == this)
        {
            currentEnemies--;
        }
    }
}