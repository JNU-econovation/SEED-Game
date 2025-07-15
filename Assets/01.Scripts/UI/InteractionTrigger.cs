using SojaExiles;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject clueBox;
    
    public GameObject interactionUI; // "F키를 눌러 상호작용" 텍스트
    public GameObject ComputerPanel;
    public GameObject ClubDoor;
    public Transform Player2;
    public CloseComputer closeComputer;
    public TextManager TextManager;

    public KeyCode interactionKey = KeyCode.F;

    private WeaponTrigger weaponTrigger;
    private bool isPlayerInRange = false;
    private bool isClicked = false;

    private string message;

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
            TextManager.ShowClueMessage(message);


            if (gameObject.CompareTag("ComputerPuzzle"))
            {
                interactionUI.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionUI.SetActive(true);
        }
        
        if (tag == "ClubDoor")
        {
            if (closeComputer.AlreadySignIn)
            {
                interactionUI.SetActive(false);
            }
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

            if (tag == "Weapon")
            {
                message = "무기를 획득했다!";
            }
            else if (tag == "ComputerPuzzle")
            {
                Time.timeScale = 0f;
                ComputerPanel.SetActive(true);
            }
            else if (tag == "ClubDoor")
            {
                if (closeComputer.AlreadySignIn)
                {
                    message = "";
                    opencloseDoor opencloseDoor = ClubDoor.GetComponent<opencloseDoor>();
                    opencloseDoor.Player = Player2;
                }
                else
                {
                    message = "열리지 않는다.";
                }
            }
            else if (tag == "CardKeyUse")
            {
                // if (카드키 있으면) message = "키카드 사용에 성공하였습니다"
                // else message = "카드키가 없습니다"
                message = "카드키가 없습니다.";
            }
            else
            {
                message = "단서를 획득했다!";
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