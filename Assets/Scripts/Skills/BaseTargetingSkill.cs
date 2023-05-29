using System;
using Entities;
using Entities.Hero;
using UnityEngine;

namespace Skills
{
    public abstract class BaseTargetingSkill : BaseSkill
    {
        public                       int      cooldown;
        public                       bool     appliesStun;
        [Header("Targeting")] public bool     targetsWholeGroup;
        public                       int      targetsFlat = 1;
        public                       float    targetsHeroScaling;
        public                       bool     autoTargeting;

        public abstract              Factions TargetableFaction { get; }

        public int GetTargets(BaseUnit unit) => targetsFlat + (int)(targetsHeroScaling * unit.level);

        public override string GetTooltip(BaseHero selectedHero, string damage = "0-0") => base.GetTooltip(selectedHero, damage) +
                                                                                           GetManacostText(selectedHero) +
                                                                                           $"Targets:\t{GetTargets(selectedHero)}" + Environment.NewLine;

        public abstract string Activate(BaseUnit actor, BaseUnit target);
        private string GetManacostText(BaseHero hero)
        {
            if (Manacost == 0)
                return string.Empty;

            var hexColor = Manacost > hero.CurrentMana ? Konstanten.NotEnoughManaColor : Konstanten.EnoughManaColor;

            return $"Manacost:\t<b><color={hexColor}>{Manacost}</color></b>{Environment.NewLine}";
        }
    }
}