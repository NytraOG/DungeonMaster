namespace Skills.BaseSkills
{
    public abstract class HealBaseSkill : BaseSkill
    {
        public override skillCategory    Category    => skillCategory.Buff;
        public override skillSubCategory SubCategory => skillSubCategory.Heal;
    }
}