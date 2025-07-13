using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueDescriptionPanel : MonoBehaviour
{
    private ClueInfos clueInfos;

    [SerializeField] private Image clueImage;
    [SerializeField] private TextMeshProUGUI clueName;
    [SerializeField] private TextMeshProUGUI clueDescription;

    public void ShowCLueInfos(ClueInfos clueInfos)
    {
        this.clueInfos = clueInfos;
        Init();
    }

    private void Init()
    {
        clueImage.sprite = clueInfos.clueImage;
        clueName.text = clueInfos.name;
        clueDescription.text = clueInfos.description;
    }


}
