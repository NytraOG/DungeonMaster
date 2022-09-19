using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class MagicDefenseBaseSkill : BaseSkill
    {
        public override SkillCategory    Category         => SkillCategory.Magic;
        public override SkillSubCategory SubCategory      => SkillSubCategory.DefenseSkill;
        public          int              skillDefenseRoll { get; set; }
    }
}