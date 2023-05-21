using Entities;
using UnityEngine;

public abstract class BaseUnitModifikator : ScriptableObject
{
    [Header("Attributes")] public string displayname;
    public                        float  strengthMultiplier     = 1;
    public                        float  constitutionMultiplier = 1;
    public                        float  dexterityMultiplier    = 1;
    public                        float  quicknessMultiplier    = 1;
    public                        float  intuitionMultiplier    = 1;
    public                        float  logicMultiplier        = 1;
    public                        float  wisdomMultiplier       = 1;
    public                        float  willpowerMultiplier    = 1;
    public                        float  charismaMultiplier     = 1;
    [Header("Ratings")] public    int    actionsModifier;
    public                        float  flatInititiveModifier;
    public                        float  flatDamageModifier;
    public                        float  meleeAttackratingModifier;
    public                        float  rangedAttackratingModifier;
    public                        float  magicAttackratingModifier;
    public                        float  socialAttackratingModifier;
    public                        float  meleeDefensmodifier;
    public                        float  rangedDefensemodifier;
    public                        float  magicDefensemodifier;
    public                        float  socialDefensemodifier;

    public virtual void ApplyAttributeModifier(BaseUnit creature)
    {
        creature.Strength     *= (int)strengthMultiplier;
        creature.Constitution *= (int)constitutionMultiplier;
        creature.Dexterity    *= (int)dexterityMultiplier;
        creature.Quickness    *= (int)quicknessMultiplier;
        creature.Intuition    *= (int)intuitionMultiplier;
        creature.Logic        *= (int)logicMultiplier;
        creature.Wisdom       *= (int)wisdomMultiplier;
        creature.Willpower    *= (int)willpowerMultiplier;
        creature.Charisma     *= (int)charismaMultiplier;
    }

    public virtual void ApplyRatingModifier(BaseUnit unit)
    {
        unit.MeleeAttackratingModifier  += meleeAttackratingModifier;
        unit.RangedAttackratingModifier += rangedAttackratingModifier;
        unit.MagicAttackratingModifier  += magicAttackratingModifier;
        unit.SocialAttackratingModifier += socialAttackratingModifier;

        unit.MeleeDefensmodifier   += meleeDefensmodifier;
        unit.RangedDefense         += rangedDefensemodifier;
        unit.MagicDefensemodifier  += magicDefensemodifier;
        unit.SocialDefensemodifier += socialDefensemodifier;
    }

    public virtual void ApplyDamageModifier(BaseUnit creature)
    {
        creature.InitiativeFlatAdded += flatInititiveModifier;
        creature.AktionenGesamt      += actionsModifier;
        creature.AktionenAktuell     =  creature.AktionenGesamt;
        creature.FlatDamageModifier  += flatDamageModifier;
    }
}