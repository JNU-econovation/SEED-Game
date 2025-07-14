using UnityEngine;

public class BossEntranceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameObject bossCam; 
    [SerializeField] private AudioSource entranceSound;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject playerObject;
    private PlayerMovement playerMovement;

    private bool hasTriggered = false;

    private void Start()
    {
        if (playerObject != null)
            playerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            // ✅ 보스 활성화 및 애니메이션
            bossObject.SetActive(true);
            bossAnimator.SetTrigger("Enter");
            entranceSound.Play();

            // ✅ BGM 전환
            AudioManager.Instance.ChangeToBossBGM();

            // ✅ UI 끄기
            if (playerUI != null)
                playerUI.SetActive(false);

            // ✅ 플레이어 움직임 끄기
            if (playerMovement != null)
                playerMovement.enabled = false;

            // ✅ 보스 연출용 카메라 활성화
            bossCam.SetActive(true);

            // ✅ 일정 시간 후 컨트롤 복구
            StartCoroutine(ReturnControlAfterDelay(3.0f));
        }
    }

    private System.Collections.IEnumerator ReturnControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // UI 켜기
        if (playerUI != null)
            playerUI.SetActive(true);

        // 플레이어 움직임 켜기
        if (playerMovement != null)
            playerMovement.enabled = true;

        // 카메라 복귀
        bossCam.SetActive(false);
    }
}
