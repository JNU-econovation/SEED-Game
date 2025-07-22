using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float healAmount = 5f;
    [SerializeField] private float healInterval = 1f;
    private float healTimer;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            healTimer += Time.deltaTime;
            if (healTimer >= healInterval)
            {
                playerHealth.Heal(healAmount);
                healTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healTimer = 0f;
        }
    }
}
