using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class MagicWeaponBaseSkill : BaseSkill
    {
        public override SkillCategory    Category        => SkillCategory.Magic;
        public override SkillSubCategory SubCategory     => SkillSubCategory.WeaponSkill;
        public          int              skillAttackRoll { get; set; }
        public          float            skillDamageRoll { get; set; }
    }
}