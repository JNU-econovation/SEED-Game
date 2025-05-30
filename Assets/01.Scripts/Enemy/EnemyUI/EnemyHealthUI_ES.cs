using UnityEngine;

public class EnemyHealthUI: MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    [SerializeField] private RectTransform healthBarFill; // Fill 이미지의 RectTransform
    [SerializeField] private GameObject healthBarUI;

    private float originalWidth; // 시작 너비 기억
    private float timeSinceLastHit = 0f;
    private bool isRecovering = false;
    private float recoverySpeed = 10f;
    private float hideDelay = 1f;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarFill != null)
            originalWidth = healthBarFill.sizeDelta.x;

        healthBarUI.SetActive(false);
    }

    void Update()
    {
        if (currentHealth < maxHealth)
        {
            timeSinceLastHit += Time.deltaTime;
            if (timeSinceLastHit >= 5f)
                isRecovering = true;
        }

        if (isRecovering)
        {
            RecoverHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("피격됨: " + damage);

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);

        UpdateHealthBar();

        healthBarUI.SetActive(true);
        timeSinceLastHit = 0f;
        isRecovering = false;
        CancelInvoke(nameof(HideHealthBar));
    }

    void RecoverHealth()
    {
        currentHealth += recoverySpeed * Time.deltaTime;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            isRecovering = false;
            Invoke(nameof(HideHealthBar), hideDelay);
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float ratio = currentHealth / maxHealth;
        Vector2 newSize = healthBarFill.sizeDelta;
        newSize.x = originalWidth * ratio;
        healthBarFill.sizeDelta = newSize;
    }

    void HideHealthBar()
    {
        healthBarUI.SetActive(false);
    }
}
