using UnityEngine;
using UnityEngine.UI;

public class PasswordTrigger : MonoBehaviour
{
    public GameObject passwordPanel; // 키패드 UI
    public GameObject interactionUI;     // "F 키를 눌러 상호작용하세요" 텍스트
    private bool isPlayerNear = false;
    private bool isUnlocked = false;

    void Update()
    {
        if (isPlayerNear && !isUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                passwordPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else if (!isPlayerNear)
        {
            interactionUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUnlocked)
        {
            isPlayerNear = true;
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionUI.SetActive(false);
        }
    }

    public void Unlock()
    {
        isUnlocked = true;
        interactionUI.SetActive(false);
    }
}