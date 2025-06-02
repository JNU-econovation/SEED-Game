using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 콜라이더가 부모 오브젝트를 가지고 있는지 체크
        if (other.transform.parent != null)
        {
            GameObject parentObject = other.transform.parent.gameObject;

            // 부모 오브젝트가 "Enemy" 태그인지 확인
            if (parentObject.CompareTag("Enemy"))
            {
                EnemyHealthUI enemyHealth = parentObject.GetComponentInChildren<EnemyHealthUI>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(20f);
                }
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyHealthUI enemyHealth = other.GetComponentInChildren<EnemyHealthUI>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(20f);
                }
            }
        }
    }
}