using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class PassivBaseSkill : BaseSkill
    {
        public override SkillCategory    Category    => SkillCategory.Passiv;
        public override SkillSubCategory SubCategory => SkillSubCategory.Buff;
    }
}