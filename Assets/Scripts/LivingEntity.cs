using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float currentHealth { get; protected set; }
    public bool isDead { get; protected set; }
    
    public event Action onDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        onDeath();
        isDead = true;
    }
}
