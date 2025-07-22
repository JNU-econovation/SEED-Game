using UnityEngine;

public class BossEntranceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameObject bossCam;
    [SerializeField] private AudioSource entranceSound;
    [SerializeField] private GameObject bossDoor;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject playerObject;
    
    [SerializeField] private GameObject bossHealthUI;
    private BossSkill bossSkill;
    private PlayerMovement playerMovement;

    private bool hasTriggered = false;


    private void Start()
    {
        if (playerObject != null)
            playerMovement = playerObject.GetComponent<PlayerMovement>();
        bossSkill = bossObject.GetComponent<BossSkill>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            bossObject.SetActive(true);
            bossAnimator.SetTrigger("Enter");
            entranceSound.Play();

            AudioManager.Instance.ChangeToBossBGM();

            if (playerUI != null)
                playerUI.SetActive(false);

            if (playerMovement != null)
                playerMovement.enabled = false;

            bossCam.SetActive(true);

            if (bossDoor != null)
                bossDoor.SetActive(true); 

            StartCoroutine(ReturnControlAfterDelay(3.0f));
        }
    }

    private System.Collections.IEnumerator ReturnControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerUI != null)
            playerUI.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (bossHealthUI != null)
            bossHealthUI.SetActive(true);
        
        bossCam.SetActive(false);
        
        if (bossSkill != null)
            bossSkill.bossTriggered = true;
    }

    public void ResetTrigger()
    {
        hasTriggered = false;

        if (bossCam != null)
            bossCam.SetActive(false);

        if (bossAnimator != null)
            bossAnimator.ResetTrigger("Enter");

        if (playerUI != null)
            playerUI.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = true;
        if (bossDoor != null)
            bossDoor.SetActive(false);
        
        if (bossHealthUI != null)
            bossHealthUI.SetActive(false);
    }

}
