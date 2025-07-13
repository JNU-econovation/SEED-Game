using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClueInfos", menuName = "ScriptableObject/ClueInfos")]
public class ClueInfos : ScriptableObject
{
    public string name;
    public string description;
    public Sprite clueImage;
    // clueIndex가 0이면 완성 단서
    public int clueIndex;
    // 조각 단서일 때 참조할 단서
    public ClueInfos completeClue;
}
