namespace Skills.BaseSkills
{
    public abstract class MeleeDefenseBaseSkill : BaseSkill
    {
        public override skillCategory    Category         => skillCategory.Melee;
        public override skillSubCategory SubCategory      => skillSubCategory.DefenseSkill;
        public          int              skillDefenseRoll { get; set; }
    }
}