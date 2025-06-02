using System;
using UnityEngine;

public class HitboxRelay : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        enemyHealth?.OnHitboxTriggerEnter(other);
    }
}
