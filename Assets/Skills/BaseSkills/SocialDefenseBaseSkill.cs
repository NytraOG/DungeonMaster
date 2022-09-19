namespace Skills.BaseSkills
{
    public abstract class SocialDefenseBaseSkill : BaseSkill
    {
        public override skillCategory    Category         => skillCategory.Social;
        public override skillSubCategory SubCategory      => skillSubCategory.DefenseSkill;
        public          int              skillDefenseRoll { get; set; }
    }
}