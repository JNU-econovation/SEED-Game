using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealthUI enemyHealth = other.GetComponentInChildren<EnemyHealthUI>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20f); // 데미지 값은 상황에 맞게 조정
            }
        }
    }
}

