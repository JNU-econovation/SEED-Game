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
        messageObject.SetActive(true);
        messageText.text = message;
        messageText.alpha = 1f;

        // ✅ 실제 시간 기준으로 대기
        yield return new WaitForSecondsRealtime(displayTime);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // ✅ timeScale 무시하고 시간 증가
            messageText.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        messageText.alpha = 0f;
        messageObject.SetActive(false);
    }

}