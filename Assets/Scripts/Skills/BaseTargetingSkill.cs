using Entities;

namespace Skills
{
    public abstract class BaseTargetingSkill : BaseSkill
    {
        public          int      targetsFlat = 1;
        public          float    targetsHeroScaling;
        public abstract Factions TargetableFaction { get; }

        public int GetTargets(BaseUnit unit) => targetsFlat + (int)(targetsHeroScaling * unit.level);
    }
}