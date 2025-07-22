using UnityEngine;

public class BossPuzzleManager : MonoBehaviour
{
    private BossSkill bossSkill;
    public SmashPuzzle smashPuzzle;
    public SequencePuzzle sequencePuzzle;


    private void Awake()
    {
        bossSkill = GetComponent<BossSkill>();
    }

    public void StartPuzzle()
    {
        Debug.Log("즉사 퍼즐 전체 시작!");
        smashPuzzle.StartPuzzle();
    }

    public void PuzzleSuccess()
    {
        Debug.Log("즉사 퍼즐 최종 성공!");
        bossSkill.PuzzleSuccess();
    }

    public void PuzzleFailed()
    {
        Debug.Log("즉사 퍼즐 최종 실패!");
        bossSkill.PuzzleFailed();
    }
}
