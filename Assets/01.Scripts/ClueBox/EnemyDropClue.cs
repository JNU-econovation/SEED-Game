using System.Collections.Generic;
using UnityEngine;

public class EnemyDropClue : MonoBehaviour
{
    [SerializeField] List<GameObject> clue;
    [SerializeField] private float dropChance = 0.3f;
    
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        enemyHealth.onDeath += Drop;    
    }

    private void OnDisable()
    {
        enemyHealth.onDeath -= Drop;  
    }

    private void Drop()
    {
        if (Random.value < dropChance)
        {
            int randomIndex = Random.Range(0, clue.Count);
            Instantiate(clue[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
