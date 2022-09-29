using System;
using Entities;
using Entities.Classes;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        private         Random rng;
        public override float  Schadensmodifier => 5.25f;

        // Start is called before the first frame update
        private void Start()
        {
            Strength     = 5;
            Constitution = 3;
            Dexterity    = 9;
            Quickness    = 8;
            Intuition    = 8;
            Logic        = 5;
            Willpower    = 2;
            Wisdom       = 5;
            Charisma     = 1;
            Schaden      = 10;
            rng          = new Random();
        }

        // Update is called once per frame
        private void Update() { }

        public override int DealDamage(BaseUnit target)
        {
            // rng = new Random();
            var modifier    = rng.Next(0, 2);
            var damageDealt = Schaden * Schadensmodifier * modifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}