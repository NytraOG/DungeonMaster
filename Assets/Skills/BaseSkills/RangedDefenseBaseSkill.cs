using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class RangedDefenseBaseSkill : BaseSkill
    {
        public override SkillCategory    Category         => SkillCategory.Ranged;
        public override SkillSubCategory SubCategory      => SkillSubCategory.DefenseSkill;
        public          int              skillDefenseRoll { get; set; }
    }
}