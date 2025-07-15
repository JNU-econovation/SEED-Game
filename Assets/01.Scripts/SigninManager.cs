using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
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

    [SerializeField] private CloseComputer CloseComputer;

    void Start()
    {
        OpenClubDoorMessage.text = "동아리 방의 문에서 찰칵하는 소리가 들렸다.";
    }

    public void TrySignIn()
    {
        string input = passwordInput.text;

        if (input == correctPassword)
        {
            SigninPanel.SetActive(false);
            ComputerBGPanel.SetActive(true);
            CloseComputer.AlreadySignIn = true;
            StartCoroutine(FadeOpenClubDoorMessage(OpenClubDoorMessage.text));
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