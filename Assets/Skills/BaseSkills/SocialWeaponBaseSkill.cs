using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class SocialWeaponBaseSkill : BaseSkill
    {
        public override SkillCategory    Category        => SkillCategory.Social;
        public override SkillSubCategory SubCategory     => SkillSubCategory.WeaponSkill;
        public          int              skillAttackRoll { get; set; }
        public          float            skillDamageRoll { get; set; }
    }
}