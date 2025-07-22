using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.BoolParameter;

public class SigninManager : MonoBehaviour
{
    public TMP_InputField passwordInput;
    public TextMeshProUGUI placeholderText;
    public TextMeshProUGUI OpenClubDoorMessage;
    public GameObject SigninPanel;
    public GameObject ComputerBGPanel;
    public float displayTime = 2f;
    public float fadeDuration = 1f;
    private string correctPassword = "ecnv";
    private bool sovlePassword = false;

    [SerializeField] private CloseComputer CloseComputer;

    void Start()
    {
        OpenClubDoorMessage.text = "동아리 방의 문에서 찰칵하는 소리가 들렸다.";
    }

    private void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if ((Input.GetKeyDown(KeyCode.Return) && selected == passwordInput.gameObject) && !sovlePassword)
        {
            TrySignIn();
        }
    }

    public void TrySignIn()
    {
        string input = passwordInput.text;

        if (input == correctPassword)
        {
            SigninPanel.SetActive(false);
            ComputerBGPanel.SetActive(true);
            CloseComputer.AlreadySignIn = true;
            sovlePassword = true;
            StartCoroutine(FadeOpenClubDoorMessage(OpenClubDoorMessage.text));
            AudioManager.Instance.PlayEastRoomOpen();
        }
        else
        {
            passwordInput.text = "";
            placeholderText.text = "비밀번호를 잘못 입력하셨습니다.";
        }
    }

    IEnumerator FadeOpenClubDoorMessage(string message)
    {
        OpenClubDoorMessage.gameObject.SetActive(true);
        OpenClubDoorMessage.text = message;
        OpenClubDoorMessage.alpha = 1f;

        yield return new WaitForSecondsRealtime(displayTime);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            OpenClubDoorMessage.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        OpenClubDoorMessage.alpha = 0f;
        OpenClubDoorMessage.gameObject.SetActive(false);
    }

}