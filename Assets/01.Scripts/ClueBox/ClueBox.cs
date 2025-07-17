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
        var clueDict = CheckClue();

        bool isMerged = false; 

        foreach (var (completeClueInfos, pieces) in clueDict)
        {
            if (pieces.Count == 4)
            {
                foreach (ClueInfos p in pieces)
                {
                    RemoveClue(p);
                }
                AddClue(completeClueInfos);

                isMerged = true; 
            }
        }

        // 합쳐졌을 때만 소리 재생
        if (isMerged)
        {
            AudioManager.Instance.PlayClueCombine();
        }
    }

    public void LoseClue()
    {
        var clueDict = CheckClue();
        if (clueDict.Count == 0) return;

        // 완성단서인 key 한 개 랜덤 추출
        List<ClueInfos> keys = new(clueDict.Keys);
        ClueInfos randomKey = keys[Random.Range(0, keys.Count)];

        // 조각 단서 하나 추출
        List<ClueInfos> pieces = clueDict[randomKey];
        if (pieces.Count == 0) return;

        // 조각 단서 삭제
        ClueInfos randomPiece = pieces[Random.Range(0, pieces.Count)];
        RemoveClue(randomPiece);
    }
    private Dictionary<ClueInfos, List<ClueInfos>> CheckClue()
    {
        Dictionary<ClueInfos, List<ClueInfos>> clueDict = new();
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

        return clueDict;
    }
}