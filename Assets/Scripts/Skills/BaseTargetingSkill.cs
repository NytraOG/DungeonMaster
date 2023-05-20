using System;
using Entities;
using Entities.Hero;

namespace Skills
{
    public abstract class BaseTargetingSkill : BaseSkill
    {
        public          int      targetsFlat = 1;
        public          float    targetsHeroScaling;
        public abstract Factions TargetableFaction { get; }

        public int GetTargets(BaseUnit unit) => targetsFlat + (int)(targetsHeroScaling * unit.level);

        public override string GetTooltip(BaseHero selectedHero, string damage = "0-0") => base.GetTooltip(selectedHero, damage) +
                                                                                           $"Targets:\t{GetTargets(selectedHero)}" +
                                                                                           Environment.NewLine + Environment.NewLine +
                                                                                           Description;
    }
}