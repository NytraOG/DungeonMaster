using Entities.Enums;
using Skills.BaseSkills;

namespace Skills
{
    public class ChainWeapons : MeleeWeaponBaseSkill
    {
        public ChainWeapons() =>
                // skillAttackRoll = 2 * Dexterity + Quickness + 2 * skillLevel;
                // skillDefenseRoll = 2 * Quickness + Strength + 2 * skillLevel * 0,9;
                // skillDamageRoll = Strength / 2 + Quickness / 3 + skillLevel / 2;
                SkillManaCost = 0 * SkillLevel;

        public override SkillItemUsed       ItemUsed       => SkillItemUsed.Chainweapon;
        public override SkillTargetPosition TargetPosition => SkillTargetPosition.Position;
        public override SkillDifficulty     Difficulty     => SkillDifficulty.Basic;
        public override SkillProvidedBy     ProvidedBy     => SkillProvidedBy.Energist;
        public override SkillKeyword        Keyword        => SkillKeyword.Chainweapon;

        public void Start()
        {
            Name        = "Chain Weapons";
            Description = "Weapons like chains, throwing hooks but also whips belong to this category.";
        }
    }
}