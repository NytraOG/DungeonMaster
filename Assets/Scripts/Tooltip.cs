using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public        RectTransform   canvasRectTransform;
    private       RectTransform   backgroundTransform;
    private       RectTransform   rectTransform;
    private       TextMeshProUGUI text;
    public static Tooltip         Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        rectTransform       = transform.GetComponent<RectTransform>();
        backgroundTransform = transform.Find("Background").GetComponent<RectTransform>();
        text                = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundTransform.rect.width > canvasRectTransform.rect.width)
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundTransform.rect.width;

        if (anchoredPosition.y + backgroundTransform.rect.height > canvasRectTransform.rect.height)
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundTransform.rect.height;

        rectTransform.anchoredPosition = anchoredPosition;
    }

    public void RenderTooltip(string content)
    {
        text.SetText(content);
        text.ForceMeshUpdate();

        var padding = new Vector2(8, 8);

        backgroundTransform.sizeDelta = text.GetRenderedValues(false) + padding;
    }

    public static void ShowTooltip(string message)
    {
        Instance.gameObject.SetActive(true);
        Instance.RenderTooltip(message);
    }

    public static void HideTooltip() => Instance.gameObject.SetActive(false);
}