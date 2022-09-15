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
            Stärke           = 1;
            Konstitution     = 1;
            Geschicklichkeit = 1;
            Schnelligkeit    = 1;
            Intuition        = 2;
            Logik            = 1;
            Willenskraft     = 1;
            Weisheit         = 1;
            Charisma         = 1;
            Lebenspunkte     = 10;
            Schaden          = 4;
        }

        // Update is called once per frame
        private void Update() { }

        public override int DealDamage(BaseUnit target)
        {
            var damageDealt = Schaden * Schadensmodifier;
            target.Lebenspunkte -= damageDealt;

            return (int)damageDealt;
        }
    }
}