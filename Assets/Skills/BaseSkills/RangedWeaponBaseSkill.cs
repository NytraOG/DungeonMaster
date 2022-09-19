namespace Skills.BaseSkills
{
    public abstract class RangedWeaponBaseSkill : BaseSkill
    {
        public override skillCategory    Category        => skillCategory.Ranged;
        public override skillSubCategory SubCategory     => skillSubCategory.WeaponSkill;
        public          int              skillAttackRoll { get; set; }
        public          float            skillDamageRoll { get; set; }
    }
}