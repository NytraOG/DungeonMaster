namespace Skills.BaseSkills
{
    public abstract class DungeonBuffBaseSkill : BaseSkill
    {
        public override skillCategory    Category    => skillCategory.Buff;
        public override skillSubCategory SubCategory => skillSubCategory.DungeonBuff;
    }
}