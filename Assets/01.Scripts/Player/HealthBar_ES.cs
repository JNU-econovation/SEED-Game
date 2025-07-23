using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_ES : MonoBehaviour
{
    [SerializeField] Image healthBarFillImage;
    
    private float maxHealth;
    private float currentHealth;

    public void Init(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercent = currentHealth / maxHealth;
        healthBarFillImage.fillAmount = healthPercent;
    }
}
