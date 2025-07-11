using System.Collections.Generic;
using UnityEngine;

public class ClueListPanel : MonoBehaviour
{
    [SerializeField] private ClueBox clueBox;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cluListButtonPrefab;
    
    
    private void OnEnable()
    {
        HashSet<ClueInfos> childClueInfosSet = new HashSet<ClueInfos>();
        foreach (Transform child in content)
        {
            ClueListButton clueListButton = child.GetComponent<ClueListButton>();
            childClueInfosSet.Add(clueListButton.clueInfos);
        }
        
        foreach (ClueInfos clueInfos in clueBox.GetClueInfos())
        {
            if (!childClueInfosSet.Contains(clueInfos))
            {
                GameObject clueListButton = Instantiate(cluListButtonPrefab, content);
                clueListButton.GetComponent<ClueListButton>().Init(clueInfos);
            }
        }
    }
}
