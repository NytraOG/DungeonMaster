using System;
using Entities;
using Entities.Classes;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        private         Random rng;
        public override float  Schadensmodifier => 1.25f;

        // Start is called before the first frame update
        private void Start()
        {
            rng       = new Random();
            Intuition = 2;
            Charisma  = 1;
            Hitpoints = HitpointsMax;
            Schaden   = 4;
        }

        // Update is called once per frame
        private void Update() { }

        public override int DealDamage(BaseUnit target)
        {
            var modifier    = rng.Next(0, 2);
            var damageDealt = Schaden * Schadensmodifier * modifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}