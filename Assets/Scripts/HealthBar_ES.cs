using UnityEngine;
using UnityEngine.UI;

public class HealthBar_ES : MonoBehaviour
{
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
        // 스페이스바 누르면 데미지 10
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }

        // 좌클릭 누르면 체력 회복 5
        if (Input.GetMouseButtonDown(0))  // 0: 왼쪽 클릭
        {
            Heal(5f);
        }
    }

    void TakeDamage(float damage)
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
