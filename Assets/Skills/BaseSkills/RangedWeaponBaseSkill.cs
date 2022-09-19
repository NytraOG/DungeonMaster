using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class RangedWeaponBaseSkill : BaseSkill
    {
        public override SkillCategory    Category        => SkillCategory.Ranged;
        public override SkillSubCategory SubCategory     => SkillSubCategory.WeaponSkill;
        public          int              skillAttackRoll { get; set; }
        public          float            skillDamageRoll { get; set; }
    }
}