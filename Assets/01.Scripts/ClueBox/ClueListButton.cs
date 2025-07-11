using TMPro;
using UnityEngine;

public class ClueListButton : MonoBehaviour
{
    public ClueInfos clueInfos { get; private set; }

    private TextMeshPro clueName;
    private Sprite clueImage;

    public void Init(ClueInfos clueInfos)
    {
        this.clueInfos = clueInfos;
        
        clueName.text = clueInfos.name;
        clueImage = clueInfos.clueImage;
    }

}
