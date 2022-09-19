using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class HealBaseSkill : BaseSkill
    {
        public override SkillCategory    Category    => SkillCategory.Buff;
        public override SkillSubCategory SubCategory => SkillSubCategory.Heal;
    }
}