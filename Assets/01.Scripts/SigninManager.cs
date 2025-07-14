using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SigninManager : MonoBehaviour
{
    public TMP_InputField passwordInput;
    public TextMeshProUGUI placeholderText;

    public GameObject SigninPanel;
    public GameObject ComputerBGPanel;
    private string correctPassword = "ecnv";

    [SerializeField] private CloseComputer CloseComputer;

    public void TrySignIn()
    {
        string input = passwordInput.text;

        if (input == correctPassword)
        {
            SigninPanel.SetActive(false);
            ComputerBGPanel.SetActive(true);
            CloseComputer.AlreadySignIn = true;
        }
        else
        {
            passwordInput.text = "";
            placeholderText.text = "비밀번호를 잘못 입력하셨습니다.";

            // 2초 뒤에 placeholder 초기화
            StartCoroutine(ClearPlaceholderAfterDelay(2f));
        }
    }

    private IEnumerator ClearPlaceholderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        placeholderText.text = ""; // 비우기
    }
}

