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
        var healthUpdated = (int)unit.Hitpoints != (int)currentHealth;

        if (healthUpdated)
        {
            greenBar.fillAmount = unit.Hitpoints / unit.MaximumHitpoints;
            currentHealth       = unit.Hitpoints;
        }
    }
}