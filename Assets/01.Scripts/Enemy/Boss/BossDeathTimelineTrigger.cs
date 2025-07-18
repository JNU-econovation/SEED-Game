using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BossDeathTimelineTrigger : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public PlayableDirector timeline;

    public GameObject player;     
    public GameObject hudUI;    
    public GameObject bossHealthUI;  
    public GameObject clearUI;    

    public float delay = 2f;
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && enemyHealth != null && enemyHealth.IsDead())
        {
            hasTriggered = true;
            StartCoroutine(PlayTimelineSequence());
        }
    }

    IEnumerator PlayTimelineSequence()
    {
        if (player != null) player.SetActive(false);
        if (hudUI != null) hudUI.SetActive(false);
        if (bossHealthUI != null) bossHealthUI.SetActive(false);

        yield return new WaitForSeconds(delay);

        timeline.stopped += OnTimelineFinished;
        timeline.Play();
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        timeline.stopped -= OnTimelineFinished;

        if (clearUI != null) clearUI.SetActive(true);
    }
}
