using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SmashPuzzle : MonoBehaviour
{
    public int requiredPresses = 10;
    public float timeLimit = 5f;
    private int currentPresses = 0;
    private bool isActive = false;

    public GameObject smashPanel;
    public Slider gaugeSlider; // Slider 기반 게이지

    private BossPuzzleManager puzzleManager;

    private void Start()
    {
        puzzleManager = FindObjectOfType<BossPuzzleManager>();
    }

    public void StartPuzzle()
    {
        currentPresses = 0;
        isActive = true;
        Debug.Log("연타 퍼즐 시작!");

        smashPanel.SetActive(true);

        if (gaugeSlider != null)
            gaugeSlider.value = 0f;

        // 씬 정지
        Time.timeScale = 0f;

        // 실제 시간 기준 실패 타이머 시작
        StartCoroutine(FailAfterRealTime(timeLimit));
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPresses++;
            Debug.Log("현재 연타 횟수: " + currentPresses);

            // 게이지 업데이트
            if (gaugeSlider != null)
                gaugeSlider.value = Mathf.Clamp01((float)currentPresses / requiredPresses);

            if (currentPresses >= requiredPresses)
            {
                SuccessPuzzle();
            }
        }
    }

    private IEnumerator FailAfterRealTime(float seconds)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - start < seconds && isActive)
        {
            yield return null;
        }
        if (isActive)
        {
            FailPuzzle();
        }
    }

    private void SuccessPuzzle()
    {
        isActive = false;
        Time.timeScale = 1f; // 씬 재개
        Debug.Log("연타 퍼즐 성공!");
        smashPanel.SetActive(false);

        if (puzzleManager != null && puzzleManager.sequencePuzzle != null)
            puzzleManager.sequencePuzzle.StartPuzzle();
        else
            Debug.LogWarning("SequencePuzzle 연결 안 됨!");
    }

    private void FailPuzzle()
    {
        isActive = false;
        Time.timeScale = 1f; // 씬 재개
        smashPanel.SetActive(false);
        Debug.Log("연타 퍼즐 실패!");

        if (puzzleManager != null)
            puzzleManager.PuzzleFailed();
    }
}
