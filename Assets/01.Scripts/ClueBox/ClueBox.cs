using System.Collections.Generic;
using UnityEngine;

public class ClueBox : MonoBehaviour
{
    private HashSet<ClueInfos> clueInfosSet = new HashSet<ClueInfos>();

    public IEnumerable<ClueInfos> GetClueInfos()
    {
        return clueInfosSet;
    }
    
    public void AddClue(ClueInfos clue)
    {
        clueInfosSet.Add(clue);
    }

    public void RemoveClue(ClueInfos clue)
    {
        clueInfosSet.Remove(clue);   
    }
}