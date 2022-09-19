using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class SocialDefenseBaseSkill : BaseSkill
    {
        public override SkillCategory    Category         => SkillCategory.Social;
        public override SkillSubCategory SubCategory      => SkillSubCategory.DefenseSkill;
        public          int              skillDefenseRoll { get; set; }
    }
}