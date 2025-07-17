using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public GameObject ClubDoor;

    public GameObject ComputerPanel;
    public Transform Player2;
    public CloseComputer closeComputer;
    public List<GameObject> SecurityGateBeams;

    private GameObject clueBox;
    private GameObject interactionUI;
    private GameObject interactionUIText;
    private TextManager TextManager;

    public KeyCode interactionKey = KeyCode.F;

    private WeaponTrigger weaponTrigger;
    private bool isPlayerInRange = false;
    private bool isClicked = false;

    private string message;

    private void Awake()
    {
        weaponTrigger = GetComponent<WeaponTrigger>();
        if (tag == "Clue" || tag == "CardKey")
        {
            clueBox = GameObject.Find("ClueBox");
        }

        interactionUI = GameObject.Find("InteractionUiCanvas");
        interactionUIText = interactionUI.transform.GetChild(0).gameObject;
        TextManager = interactionUI.GetComponent<TextManager>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            isClicked = true;
            interactionUIText.SetActive(false);
            TextManager.ShowClueMessage(message);


            if (gameObject.CompareTag("ComputerPuzzle"))
            {
                interactionUIText.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionUIText.SetActive(true);
        }
        
        if (tag == "ClubDoor")
        {
            if (closeComputer.AlreadySignIn)
            {
                interactionUIText.SetActive(false);
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

            if (tag == "CardKey" && !isClicked)
            {
                StartCoroutine(GetCardKey());
            }

            if (tag == "PlayerWeapon")
            {
                weaponTrigger.ChangeWeapon();
            }

            if (tag == "Weapon")
            {
                message = "무기를 획득했다.";
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
            else if (tag == "CardKey")
            {
                message = "카드키를 획득했다.";
                CardKeyManager.Instance.hasCardKey = true;
            }
            else if (tag == "CardKeyUse")
            {
                if (CardKeyManager.Instance.hasCardKey)
                {
                    message = "카드키를 사용하였습니다.";
                    if (SecurityGateBeams != null)
                    {
                        foreach (GameObject obj in SecurityGateBeams)
                        {
                            Transform parent = obj.transform.parent;

                            if (parent != null)
                            {
                                Collider parentCollider = parent.GetComponent<Collider>();
                                if (parentCollider != null)
                                {
                                    parentCollider.enabled = false;
                                }
                            }
                                if (obj != null)
                                obj.SetActive(false);
                        }
                    }
                }
                else
                {
                    message = "카드키가 없습니다.";
                }
            }
            else
            {
                message = "단서를 획득했다.";
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUIText.SetActive(false);
        }
    }

    private IEnumerator GetClue()
    {
        DisableInteraction();
        float totalDisplayTime = TextManager.displayTime + TextManager.fadeDuration;
        ClueMoveToClueBox();
        yield return new WaitForSeconds(totalDisplayTime);
        Destroy(gameObject);
    }

    private IEnumerator GetCardKey()
    {
        DisableInteraction();
        float totalDisplayTime = TextManager.displayTime + TextManager.fadeDuration;
        CardKeyMoveToClueBox();
        yield return new WaitForSeconds(totalDisplayTime);
        Destroy(gameObject);
    }

    private void ClueMoveToClueBox()
    {
        Clue clue = GetComponent<Clue>();
        clueBox.GetComponent<ClueBox>().AddClue(clue.clueInfos);
        
    }

    private void CardKeyMoveToClueBox()
    {
        CardKey cardKey = GetComponent<CardKey>();
        clueBox.GetComponent<ClueBox>().AddClue(cardKey.clueInfos);
    }
    
    private void DisableInteraction()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<Light>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
    }
}