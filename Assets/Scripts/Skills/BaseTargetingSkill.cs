namespace Skills
{
    public abstract class BaseTargetingSkill : BaseSkill
    {
        public          int      targets = 1;
        public abstract Factions TargetableFaction { get; }
    }
}