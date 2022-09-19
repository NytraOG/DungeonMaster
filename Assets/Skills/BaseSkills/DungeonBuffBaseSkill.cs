using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class DungeonBuffBaseSkill : BaseSkill
    {
        public override SkillCategory    Category    => SkillCategory.Buff;
        public override SkillSubCategory SubCategory => SkillSubCategory.DungeonBuff;
    }
}