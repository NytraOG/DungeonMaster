namespace Skills.BaseSkills
{
    public abstract class MagicDefenseBaseSkill : BaseSkill
    {
        public override skillCategory    Category         => skillCategory.Magic;
        public override skillSubCategory SubCategory      => skillSubCategory.DefenseSkill;
        public          int              skillDefenseRoll { get; set; }
    }
}