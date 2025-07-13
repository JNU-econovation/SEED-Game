using System.Collections;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject clueBox;
    
    public GameObject interactionUI; // "F키를 눌러 상호작용" 텍스트
    public TextManager TextManager;
    public KeyCode interactionKey = KeyCode.F;

    private WeaponTrigger weaponTrigger;
    private bool isPlayerInRange = false;
    private bool isClicked = false;


    private void Awake()
    {
        weaponTrigger = GetComponent<WeaponTrigger>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            isClicked = true;
            interactionUI.SetActive(false);
            TextManager.ShowClueMessage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;
        if (Input.GetKeyDown(interactionKey))
        {
            // 주울 수 있는 단서만 해당
            // 다른 상호작용은 없어지면 안됨
            if (tag == "Clue" && !isClicked)
            {
                StartCoroutine(GetClue());
            }

            if (tag == "PlayerWeapon")
            {
                weaponTrigger.ChangeWeapon();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUI.SetActive(false);
        }
    }

    private IEnumerator GetClue()
    {
        DisableInteraction();
        float totalDisplayTime = TextManager.displayTime + TextManager.fadeDuration;
        MoveToClueBox();
        yield return new WaitForSeconds(totalDisplayTime);
        Destroy(gameObject);
    }

    private void MoveToClueBox()
    {
        Clue clue = GetComponent<Clue>();
        clueBox.GetComponent<ClueBox>().AddClue(clue.clueInfos);
    }
    
    private void DisableInteraction()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<Light>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
    }
}

