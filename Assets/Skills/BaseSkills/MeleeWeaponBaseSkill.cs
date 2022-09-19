using Entities.Enums;

namespace Skills.BaseSkills
{
    public abstract class MeleeWeaponBaseSkill : BaseSkill
    {
        public override SkillCategory    Category         => SkillCategory.Melee;
        public override SkillSubCategory SubCategory      => SkillSubCategory.WeaponSkill;
        public override SkillRange       Range            => SkillRange.Melee;
        public          int              skillAttackRoll  { get; set; }
        public          int              skillDefenseRoll { get; set; }
        public          float            skillDamageRoll  { get; set; }
    }
}