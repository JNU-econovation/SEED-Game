using UnityEngine;

public class ChangeUICanvas : MonoBehaviour
{
    [SerializeField] private GameObject GameMainUI;
    [SerializeField] private GameObject ClueJournalUI;

    private bool isJournalOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        isJournalOpen = !isJournalOpen;

        // UI 활성화/비활성화 전환
        ClueJournalUI.SetActive(isJournalOpen);
        GameMainUI.SetActive(!isJournalOpen);

        // 시간 정지 or 재개
        Time.timeScale = isJournalOpen ? 0f : 1f;
    }
}

