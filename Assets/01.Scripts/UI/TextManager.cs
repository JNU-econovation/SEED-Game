using System.Collections;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public GameObject messageObject;      // 🔸 Message 전체 오브젝트 (Text 포함)
    public TextMeshProUGUI messageText;   // 🔸 텍스트 자체
    public float displayTime = 2f;
    public float fadeDuration = 1f;

    public void ShowClueMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(FadeClueMessage(message));
    }

    IEnumerator FadeClueMessage(string message)
    {
        messageObject.SetActive(true);        // 🔸 메시지 전체 켜기
        messageText.text = message;
        messageText.alpha = 1f;

        yield return new WaitForSeconds(displayTime);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            messageText.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        messageText.alpha = 0f;
        messageObject.SetActive(false);       // 🔸 메시지 전체 끄기
    }
}