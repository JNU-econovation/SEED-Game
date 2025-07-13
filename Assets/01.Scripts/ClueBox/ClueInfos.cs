using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClueInfos", menuName = "ScriptableObject/ClueInfos")]
public class ClueInfos : ScriptableObject
{
    public string name;
    public string description;
    public Sprite clueImage;
}
