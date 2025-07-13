using System.Collections.Generic;
using UnityEngine;

public class ClueListPanel : MonoBehaviour
{
    [SerializeField] private ClueBox clueBox;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject clueListButtonPrefab;
    [SerializeField] private ClueDescriptionPanel clueDescriptionPanel;
    
    public ClueBox ClueBox => clueBox;
    
    private void OnEnable()
    {
        UpdateClueList();
    }

    public void UpdateClueList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);       
        }

        foreach (ClueInfos clueInfos in clueBox.GetClueInfos())
        {
            GameObject clueListButton = Instantiate(clueListButtonPrefab, content);
            clueListButton.GetComponent<ClueListButton>().Init(clueInfos, clueDescriptionPanel);
        }
    }
}
