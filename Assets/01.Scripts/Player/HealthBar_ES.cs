using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_ES : MonoBehaviour
{
    [SerializeField] Image healthBarFillImage;
    
    private float maxHealth;
    private float currentHealth;

    private void OnEnable()
    {
        if (maxHealth != null)
        {
            UpdateHealthBar();
        }
    }

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
