using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class MeleeDefenseBaseSkill : BaseSkill
    {
        public override SkillCategory    Category         => SkillCategory.Melee;
        public override SkillSubCategory SubCategory      => SkillSubCategory.DefenseSkill;
        public          int              skillDefenseRoll { get; set; }
    }
}