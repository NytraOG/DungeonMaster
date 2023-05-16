using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitTooltip : Tooltip
{
    public  BaseUnit unit;
    private float    currentHealth;
    private Image    currentHealthbar;

    protected override void Awake()
    {
        currentHealthbar = transform.Find("CurrentHealth").GetComponent<Image>();

        base.Awake();
    }

    protected override void Update()
    {
        if (unit is null)
            return;

        Vector2 anchoredPosition = unit.transform.position * 100 / canvasRectTransform.localScale.x;

        anchoredPosition.y += 50;

        // if (anchoredPosition.x + BackgroundTransform.rect.width > canvasRectTransform.rect.width)
        //     anchoredPosition.x = canvasRectTransform.rect.width - BackgroundTransform.rect.width;
        //
        // if (anchoredPosition.y + BackgroundTransform.rect.height > canvasRectTransform.rect.height)
        //     anchoredPosition.y = canvasRectTransform.rect.height - BackgroundTransform.rect.height;

        RectTransform.anchoredPosition = anchoredPosition;

        transform.Find("UnitName").GetComponent<TextMeshProUGUI>().text = unit.name;

        var healthUpdated = (int)unit.CurrentHitpoints != (int)currentHealth;

        if (!healthUpdated)
            return;

        currentHealthbar.fillAmount = unit.CurrentHitpoints / unit.MaximumHitpoints;
        currentHealth               = unit.CurrentHitpoints;
    }
}