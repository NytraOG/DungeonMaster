using Entities;
using UnityEngine;
using UnityEngine.UI;

public class HealthpointBar : MonoBehaviour
{
    public  BaseUnit unit;
    private float    currentHealth;
    private Image    greenBar;

    public void Awake() => greenBar = transform.Find("Canvas").Find("CurrentHealth").GetComponent<Image>();

    public void Update()
    {
        if(unit is null)
            return;

        if (unit.IsDead)
            gameObject.SetActive(false);

        var healthUpdated = (int)unit.CurrentHitpoints != (int)currentHealth;

        if (healthUpdated)
        {
            greenBar.fillAmount = unit.CurrentHitpoints / unit.MaximumHitpoints;
            currentHealth       = unit.CurrentHitpoints;
        }
    }
}