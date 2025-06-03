using UnityEngine;
using UnityEngine.UI;

public class HealthBar_ES : MonoBehaviour
{
    // 뭐가 문제야.
    [SerializeField] RectTransform HealthBarfillRect;
    [SerializeField] float maxHealth = 100f;
    private float currentHealth;

    private float originalWidth;

    void Start()
    {
        currentHealth = maxHealth;
        originalWidth = HealthBarfillRect.sizeDelta.x;
        UpdateHealthBar();
    }

    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
    }

    void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float healthPercent = currentHealth / maxHealth;
        HealthBarfillRect.sizeDelta = new Vector2(originalWidth * healthPercent, HealthBarfillRect.sizeDelta.y);
    }
}
