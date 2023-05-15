using Entities;
using TMPro;
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
        base.Update();

        if (unit is null)
            return;

        transform.Find("UnitName").GetComponent<TextMeshProUGUI>().text = unit.name;

        var healthUpdated = (int)unit.CurrentHitpoints != (int)currentHealth;

        if (!healthUpdated)
            return;

        currentHealthbar.fillAmount = unit.CurrentHitpoints / unit.MaximumHitpoints;
        currentHealth               = unit.CurrentHitpoints;
    }
}