using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SequencePuzzle : MonoBehaviour
{
    public List<Button> buttons;
    public GameObject sequencePanel;

    private List<int> correctSequence = new List<int>();
    private List<int> playerInput = new List<int>();
    private int sequenceLength = 4;
    private bool isActive = false;

    private BossPuzzleManager puzzleManager;

    private void Start()
    {
        puzzleManager = FindObjectOfType<BossPuzzleManager>();
    }

    public void StartPuzzle()
    {
        isActive = true;
        Debug.Log("순서 퍼즐 시작!");

        sequencePanel.SetActive(true);
        Time.timeScale = 0f; // 씬 멈춤

        GenerateSequence();
        StartCoroutine(ShowSequenceCoroutine());
    }

    private void GenerateSequence()
    {
        correctSequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            correctSequence.Add(Random.Range(0, buttons.Count));
        }
    }

    private IEnumerator ShowSequenceCoroutine()
    {
        for (int i = 0; i < correctSequence.Count; i++)
        {
            int idx = correctSequence[i];
            buttons[idx].GetComponent<Image>().color = Color.yellow;
            yield return new WaitForSecondsRealtime(0.5f);
            buttons[idx].GetComponent<Image>().color = Color.white;
            yield return new WaitForSecondsRealtime(0.2f);
        }

        EnableButtons();
    }

    private void EnableButtons()
    {
        playerInput.Clear();
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int idx)
    {
        if (!isActive) return;

        playerInput.Add(idx);

        if (playerInput.Count == correctSequence.Count)
        {
            CheckResult();
        }
    }

    private void CheckResult()
    {
        isActive = false;
        sequencePanel.SetActive(false);
        Time.timeScale = 1f; // 씬 재개

        bool correct = true;
        for (int i = 0; i < correctSequence.Count; i++)
        {
            if (correctSequence[i] != playerInput[i])
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            Debug.Log("순서 퍼즐 성공!");
            puzzleManager.PuzzleSuccess();
        }
        else
        {
            Debug.Log("순서 퍼즐 실패!");
            puzzleManager.PuzzleFailed();
        }
    }
}
