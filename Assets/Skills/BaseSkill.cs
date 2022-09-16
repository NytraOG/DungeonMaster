using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skillCategory
{
    Melee, 
    Ranged,
    Magic,
    Social,
    Passiv,
    Summon,
    Buff,
    DungeonBuff,
    Debuff,
    Initiative
}

public enum skillSubCategory
{

}

public abstract class BaseSkill : MonoBehaviour
{
    public string Name { get; set; }
    public string Description { get; set; }

    public abstract skillCategory Category { get; }
}
