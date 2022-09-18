using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums
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
    DefenseSkill,

    WeaponSkill,

    SocialAttack,

    Buff,
    DungeonBuff,
    Debuff,
    Heal,

    Pipe,

    Energy,
}
public enum skillItemUsed
{
    None,
    
    Chainweapon,
    Knife,
    ThrowingWeapon,

    Shield,

    ShamanisticTotem
}
public enum skillDifficulty
{
    Basic,
    Demanding,
    OutOfClass,
}
public enum skillTargetPosition
{
    Self,
    Position,
    Group,
}
public enum skillRange
{
    Melee,
    One,
    Two,
    Three,
    Four,
}
public enum skillProvidedBy
{
    Orc,
    Troll,
    Kobold,
    Dwarf,
    Elf,
    Terran,
    NeoTerran,
    Ghul,

    Energy,
    Shaman,
}

#endregion

public abstract class BaseSkill : MonoBehaviour
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int skillLevel { get; set; }        //kommt ja eigentlich von woanders: Der Held/Gegner müsste den Skilllevel definieren.
    public float skillManaCost { get; set; }
    public abstract skillCategory Category { get; }
    public abstract skillSubCategory SubCategory { get; }
    public abstract skillDifficulty Difficulty { get; }
    public abstract skillItemUsed ItemUsed { get; }
    public abstract skillTargetPosition TargetPosition { get; }
    public abstract skillRange Range { get; }
    public abstract skillProvidedBy ProvidedBy { get; }
}
