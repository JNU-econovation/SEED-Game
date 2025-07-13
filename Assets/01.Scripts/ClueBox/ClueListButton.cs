using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueListButton : MonoBehaviour
{
    private ClueDescriptionPanel clueDescriptionPanel;

    public ClueInfos clueInfos { get; private set; }

    private TextMeshProUGUI clueName;
    private Image clueImage;
    
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (clueName == null)
            {
                clueName = child.GetComponent<TextMeshProUGUI>();
            }

            if (clueImage == null)
            {
                clueImage = child.GetComponent<Image>();
            }
        }
    }
    
    public void Init(ClueInfos clueInfos, ClueDescriptionPanel clueDescriptionPanel)
    {
        this.clueInfos = clueInfos;
        this.clueDescriptionPanel = clueDescriptionPanel;
        
        clueName.text = clueInfos.name;
        clueImage.sprite = clueInfos.clueImage;
    }
    
    public void OnClick()
    {
        clueDescriptionPanel.ShowCLueInfos(clueInfos);
    }
}
