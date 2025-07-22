using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class NoteUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private float widthRatio = 0.8f;
    [SerializeField] private float heightRatio = 0.6f;
    [SerializeField] private float fontSizeRatio = 0.4f;

    private RectTransform noteNumberRect;  // 이 텍스트 오브젝트의 RectTransform
    private RectTransform noteImageRect;   // 부모 이미지 오브젝트의 RectTransform

    void Awake()
    {
        noteNumberRect = GetComponent<RectTransform>();
        noteImageRect = transform.parent.GetComponent<RectTransform>();
    }

    void Start()
    {
        transform.localPosition = Vector3.zero;
        UpdateTextLayout();
    }

    public void SetNumber(int number)
    {
        noteText.text = number.ToString();
        UpdateTextLayout();
    }

    public void UpdateTextLayout()
    {
        if (noteImageRect == null)
        {
            Debug.LogWarning("부모 이미지의 RectTransform을 찾을 수 없습니다.");
            return;
        }

        float parentWidth = noteImageRect.rect.width;
        float parentHeight = noteImageRect.rect.height;

        // 텍스트 박스 크기 설정
        float textWidth = parentWidth * widthRatio;
        float textHeight = parentHeight * heightRatio;
        noteNumberRect.sizeDelta = new Vector2(textWidth, textHeight);

        // 중앙 정렬
        noteNumberRect.anchorMin = new Vector2(0.5f, 0.5f);
        noteNumberRect.anchorMax = new Vector2(0.5f, 0.5f);
        noteNumberRect.pivot = new Vector2(0.5f, 0.5f);
        noteNumberRect.anchoredPosition = Vector2.zero;

        // 폰트 크기 자동 조절
        float reference = Mathf.Min(textWidth, textHeight);
        noteText.fontSize = reference * fontSizeRatio;

        // 가운데 정렬 설정 (문자열 정렬)
        noteText.alignment = TextAlignmentOptions.Center;
    }
}
