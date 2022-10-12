using Entities.Enums;
using Skills.BaseSkills;

namespace Skills
{
    public class Knives : MeleeWeaponBaseSkill
    {
        public override SkillItemUsed       ItemUsed       => SkillItemUsed.Knife;
        public override SkillTargetPosition TargetPosition => SkillTargetPosition.Position;
        public override SkillDifficulty     Difficulty     => SkillDifficulty.Basic;
        public override SkillProvidedBy     ProvidedBy     => SkillProvidedBy.Dwarf;
        public override SkillKeyword        Keyword        => SkillKeyword.Knife;

        // Start is called before the first frame update
        private void Start()
        {
            Name        = "Knives";
            Description = "This skill indicates how well the character can handle knives or similar weapons. The essential quality is that they are sharp and small.";
        }

        // Update is called once per frame
        private void Update() { }
    }
}