using System.Collections.Generic;
using UnityEngine;

public class ClueBox : MonoBehaviour
{
    private HashSet<ClueInfos> clueInfos = new HashSet<ClueInfos>();

    public IEnumerable<ClueInfos> GetClueInfos()
    {
        return clueInfos;
    }
    
    public void AddClue(ClueInfos clue)
    {
        clueInfos.Add(clue);
    }
}