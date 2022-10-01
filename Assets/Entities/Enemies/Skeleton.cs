using System;

namespace Entities.Enemies
{
    public class Skeleton : BaseFoe
    {
        private         Random rng;
        public override float  Schadensmodifier => 1.0f;


        public override void Initialize()
        {
            rng              = new Random();
            Strength         = 5;
            Constitution     = 6;
            Dexterity        = 2;
            Quickness        = 3;
            Intuition        = 2;
            Logic            = 1;
            Willpower        = 1;
            Wisdom           = 1;
            Charisma         = 1;
            Schaden          = 6;
            
            base.Initialize();

        }

        public override int DealDamage(BaseUnit target)
        {
            var modifier    = rng.Next(0, 2);
            var damageDealt = Schaden * Schadensmodifier * modifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}