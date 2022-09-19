namespace Skills.BaseSkills
{
    public abstract class MagicWeaponBaseSkill : BaseSkill
    {
        public override skillCategory    Category        => skillCategory.Magic;
        public override skillSubCategory SubCategory     => skillSubCategory.WeaponSkill;
        public          int              skillAttackRoll { get; set; }
        public          float            skillDamageRoll { get; set; }
    }
}