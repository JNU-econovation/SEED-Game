using UnityEngine;

public class EnemyAttack_ES : MonoBehaviour
{
    private HealthBar_ES playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<HealthBar_ES>();  // 씬 내 HealthBar_ES 컴포넌트 찾아서 저장
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);
            }
        }
    }
}
