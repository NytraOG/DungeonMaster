namespace Skills.BaseSkills
{
    public abstract class DebuffBaseSkill : BaseSkill
    {
        public override skillCategory    Category    => skillCategory.Debuff;
        public override skillSubCategory SubCategory => skillSubCategory.Debuff;
    }
}