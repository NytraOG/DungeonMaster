using Skills.BaseSkills;

namespace Skills
{
    public class Knives : MeleeWeaponBaseSkill
    {
        public Knives() =>
                // skillAttackRoll = 2 * Quickness + Dexterity + 2 * skillLevel;
                // skillDefenseRoll = 2 * Dexterity + Intuition + 2 * skillLevel;
                // skillDamageRoll = Dexterity / 2 + Quickness / 3 + skillLevel / 2;
                skillManaCost = 0 * skillLevel;

        public override skillItemUsed       ItemUsed       => skillItemUsed.Knife;
        public override skillTargetPosition TargetPosition => skillTargetPosition.Position;
        public override skillDifficulty     Difficulty     => skillDifficulty.Basic;
        public override skillProvidedBy     ProvidedBy     => skillProvidedBy.Shaman;
        public override skillKeyword        Keyword        => skillKeyword.Knife;

        public void Start()
        {
            Name        = "Knives";
            Description = "This skill indicates how well the character can handle knives or similar weapons. The essential quality is that they are sharp and small.";
        }
    }
}