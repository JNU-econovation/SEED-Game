using System;
using UnityEngine;

[Obsolete]
public class EnemyHealthUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform healthBarFill; // Fill 이미지의 RectTransform

    private float originalWidth; // 시작 너비 기억
    private float recoverySpeed = 10f;
    private float hideDelay = 1f;
    private EnemyHealth enemyHealth;
    private float maxHealth;
    private float currentHealth;

    void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        maxHealth = enemyHealth.getMaxHealth();
        currentHealth = maxHealth;

        if (healthBarFill != null)
            originalWidth = healthBarFill.sizeDelta.x;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = enemyHealth.getCurrentHealth();
        UpdateHealthBar();
    }


    void UpdateHealthBar()
    {
        float ratio = currentHealth / maxHealth;
        Vector2 newSize = healthBarFill.sizeDelta;
        newSize.x = originalWidth * ratio;
        healthBarFill.sizeDelta = newSize;
    }
    public void ResetUI()
    {
        maxHealth = enemyHealth.getMaxHealth();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

}