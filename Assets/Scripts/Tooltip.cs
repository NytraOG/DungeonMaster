using Skills;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public        RectTransform       canvasRectTransform;
    protected     RectTransform       BackgroundTransform;
    protected     RectTransform       RectTransform;
    protected     TextMeshProUGUI     Text;
    public        AbilitybuttonScript displayedSkill;
    public static Tooltip             Instance { get; set; }

    protected virtual void Awake()
    {
        Instance = this;

        RectTransform       = transform.GetComponent<RectTransform>();
        BackgroundTransform = transform.Find("Background").GetComponent<RectTransform>();
        Text                = transform.Find("Text")?.GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        Vector2 anchoredPosition = displayedSkill.transform.position / canvasRectTransform.localScale.x;

        anchoredPosition.y += 37;

        if (anchoredPosition.x + BackgroundTransform.rect.width > canvasRectTransform.rect.width)
            anchoredPosition.x = canvasRectTransform.rect.width - BackgroundTransform.rect.width;

        if (anchoredPosition.y + BackgroundTransform.rect.height > canvasRectTransform.rect.height)
            anchoredPosition.y = canvasRectTransform.rect.height - BackgroundTransform.rect.height;

        RectTransform.anchoredPosition = anchoredPosition;
    }

    public void RenderTooltip(string content)
    {
        Text.SetText(content);
        Text.ForceMeshUpdate();

        var padding = new Vector2(8, 8);

        BackgroundTransform.sizeDelta = Text.GetRenderedValues(false) + padding;
    }

    public static void ShowTooltip(string message)
    {
        Instance.gameObject.SetActive(true);
        Instance.RenderTooltip(message);
    }

    public static void HideTooltip() => Instance.gameObject.SetActive(false);
}