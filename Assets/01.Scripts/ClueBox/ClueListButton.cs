using TMPro;
using UnityEngine;

public class ClueListButton : MonoBehaviour
{
    public ClueInfos clueInfos { get; private set; }

    private TextMeshPro clueName;
    private Sprite clueImage;

    private void OnEnable()
    {
        clueName.text = clueInfos.name;
        clueImage = clueInfos.clueImage;
    }

    public void UpdateClue()
    {
        
    }

    public void Init(ClueInfos clueInfos)
    {
        this.clueInfos = clueInfos;
    }

}
