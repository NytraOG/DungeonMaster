namespace Skills.BaseSkills
{
    public abstract class BuffBaseSkill : BaseSkill
    {
        public override skillCategory    Category    => skillCategory.Buff;
        public override skillSubCategory SubCategory => skillSubCategory.Buff;
    }
}