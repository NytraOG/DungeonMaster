using Entities.Enums;
using Skills.BaseSkills;

namespace Skills
{
    public class Knives : MeleeWeaponBaseSkill
    {
        public Knives() =>
                // skillAttackRoll = 2 * Quickness + Dexterity + 2 * skillLevel;
                // skillDefenseRoll = 2 * Dexterity + Intuition + 2 * skillLevel;
                // skillDamageRoll = Dexterity / 2 + Quickness / 3 + skillLevel / 2;
                SkillManaCost = 0 * SkillLevel;

        public override SkillItemUsed       ItemUsed       => SkillItemUsed.Knife;
        public override SkillTargetPosition TargetPosition => SkillTargetPosition.Position;
        public override SkillDifficulty     Difficulty     => SkillDifficulty.Basic;
        public override SkillProvidedBy     ProvidedBy     => SkillProvidedBy.Dwarf;
        public override SkillKeyword        Keyword        => SkillKeyword.Knife;

        public void Start()
        {
            Name        = "Knives";
            Description = "This skill indicates how well the character can handle knives or similar weapons. The essential quality is that they are sharp and small.";
        }
    }
}