﻿namespace Entities.Classes
{
    public sealed class Assassin : BaseHero
    {
        public override float Schadensmodifier => 1.25f;

        private void Awake()
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
            Hitpoints    = MaximumHitpoints;
            Schaden      = 1;

            InitiativeBestimmen(3);
        }

        public override int DealDamage(BaseUnit target)
        {
            var damageDealt = Schaden * Schadensmodifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}