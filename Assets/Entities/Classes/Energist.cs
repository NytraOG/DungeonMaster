﻿using System.Collections.Generic;
using Skills;
using Skills.BaseSkills;

namespace Entities.Classes
{
    public sealed class Energist : BaseHero
    {
        public Energist()
        {
            Strength     = 1;
            Constitution = 1;
            Dexterity    = 1;
            Quickness    = 1;
            Intuition    = 2;
            Logic        = 1;
            Willpower    = 1;
            Wisdom       = 1;
            Charisma     = 1;
            Hitpoints    = HitpointsMax;
            Schaden      = 1;

            InitiativeBestimmen(3);
            
            Skills.Add(Konstanten.ChainWeaponsSkill, gameObject.AddComponent<ChainWeapons>());
        }

        public override float         Schadensmodifier => 1.10f;
        public          Dictionary<string, BaseSkill> Skills           { get; set; } = new();

        public override int DealDamage(BaseUnit target)
        {
            var damageDealt = Schaden * Schadensmodifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}