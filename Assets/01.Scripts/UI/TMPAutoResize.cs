using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TMPAutoResize : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float padding = 10f;

    void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        ResizeHeight();
    }

    void ResizeHeight()
    {
        float h = tmpText.preferredHeight;

        RectTransform rt = tmpText.GetComponent<RectTransform>();
        Vector2 size = rt.sizeDelta;
        size.y = h + padding;
        rt.sizeDelta = size;
    }
}