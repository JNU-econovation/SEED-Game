using UnityEngine;

public class MergeClueButton : MonoBehaviour
{
    [SerializeField] private ClueListPanel clueListPanel;

    private ClueBox clueBox;

    private void Awake()
    {
        clueBox = clueListPanel.ClueBox;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClick()
    {
        clueBox.MergeClue();
        clueListPanel.UpdateClueList();
    }
}
