using Entities;
using Entities.Classes;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        public override float Schadensmodifier => 1.25f;

        // Start is called before the first frame update
        private void Start()
        {
            Intuition = 2;
            Charisma  = 1;
            Hitpoints = HitpointsMax;
            Schaden   = 4;
        }

        // Update is called once per frame
        private void Update() { }

        public override int DealDamage(BaseUnit target)
        {
            var damageDealt = Schaden * Schadensmodifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}