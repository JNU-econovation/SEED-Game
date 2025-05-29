using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject enemyPrefab;
    
    private int maxEnemies = 5;
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
            yield return new WaitForSeconds(5f);
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
        Instantiate(enemyPrefab);
        currentEnemies++;
    }

    void tmp(Enemy deadEnemy)
    {
        Debug.Log("Enemy is dead");
        currentEnemies--;
    }
}
