using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class DebuffBaseSkill : BaseSkill
    {
        public override SkillCategory    Category    => SkillCategory.Debuff;
        public override SkillSubCategory SubCategory => SkillSubCategory.Debuff;
    }
}