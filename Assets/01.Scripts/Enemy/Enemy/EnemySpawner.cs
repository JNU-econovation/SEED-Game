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
                Debug.Log("몬스터 스폰 됨");
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDied += tmp;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= tmp;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, randomPosition(), Quaternion.identity);
        currentEnemies++;
    }

    private Vector3 randomPosition()
    {
        // extents = size / 2
        float startX = transform.position.x - spawnArea.bounds.extents.x;
        float startZ = transform.position.z - spawnArea.bounds.extents.z;
        Vector3 spawnPosition = new Vector3(Random.Range(startX, startX + spawnArea.bounds.size.x),
            spawnArea.center.y, Random.Range(startZ, startZ + spawnArea.bounds.size.z));
        
        return spawnPosition;
    }

    void tmp(Enemy deadEnemy)
    {
        Debug.Log("Enemy is dead");
        currentEnemies--;
    }
}