using Entities;

namespace Skills.neu
{
    public abstract class BaseDamageSkill : BaseSkill
    {
        public int GetDamage(BaseUnit actor) => (int)(actor.Strength * scalingStrength +
                                                      actor.Constitution * scalingConstitution +
                                                      actor.Dexterity * scalingDexterity +
                                                      actor.Quickness * scalingQuickness +
                                                      actor.Intuition * scalingIntuition +
                                                      actor.Logic * scalingLogic +
                                                      actor.Willpower * scalingWillpower +
                                                      actor.Wisdom * scalingWisdom +
                                                      actor.Charisma * scalingCharisma) + addedFlatDamage;
    }
}