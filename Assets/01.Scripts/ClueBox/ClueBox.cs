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
        // 단서 완성품이 있을 때 단서조각 안 먹어지게
        if (clue.completeClue != null)
        {
            if (clueInfosSet.Contains(clue.completeClue))
                return;
        }
        clueInfosSet.Add(clue);
    }

    public void RemoveClue(ClueInfos clue)
    {
        clueInfosSet.Remove(clue);   
    }
    
    public void MergeClue()
    {
        Dictionary<ClueInfos, List<ClueInfos>> clueDict = new ();
        foreach (ClueInfos clue in clueInfosSet)
        {
            // 완성 단서면 합칠 필요 없음
            if (clue.clueIndex == 0) continue;
            if (!clueDict.ContainsKey(clue.completeClue))
            {
                clueDict.Add(clue.completeClue, new List<ClueInfos>());
            }
            clueDict[clue.completeClue].Add(clue);
        }

        foreach (var (completeClueInfos, pieces) in clueDict)
        {
            if (pieces.Count == 4)
            {
                foreach (ClueInfos p in pieces)
                {
                    RemoveClue(p);
                }
                AddClue(completeClueInfos);
            }
        }
    }
}