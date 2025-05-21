using UnityEngine;
using UnityEngine.UI;

public class StaminaBar_ES : MonoBehaviour
{
    [SerializeField] RectTransform StaminaBarfillRect;
    [SerializeField] float maxStamina = 100f;
    private float currentStamina;

    private float originalWidth;

    void Start()
    {
        currentStamina = maxStamina;
        originalWidth = StaminaBarfillRect.sizeDelta.x;
        UpdateStaminaBar();
    }

    void Update()
    {
        // 스페이스바 누르면 스태미나 10 소모
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseStamina(10f);
        }

        // 좌클릭 누르면 스태미나 5 회복
        if (Input.GetMouseButtonDown(0))  // 0: 왼쪽 클릭
        {
            RecoverStamina(5f);
        }
    }

    void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        UpdateStaminaBar();
    }

    void RecoverStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        UpdateStaminaBar();
    }

    void UpdateStaminaBar()
    {
        float staminaPercent = currentStamina / maxStamina;
        StaminaBarfillRect.sizeDelta = new Vector2(originalWidth * staminaPercent, StaminaBarfillRect.sizeDelta.y);
    }
}
