namespace Entities.Enemies
{
    public sealed class Goblin : BaseFoe
    {
        public Goblin()
        {
            Stärke           = 1;
            Konstitution     = 1;
            Geschicklichkeit = 1;
            Schnelligkeit    = 1;
            Intuition        = 1;
            Logik            = 1;
            Willenskraft     = 1;
            Weisheit         = 1;
            Charisma         = 1;
            Lebenspunkte     = 10;
            Schaden          = 4;

            InitiativeBestimmen();
        }

        public override float Schadensmodifier => 1f;

        public override int DealDamage(BaseUnit target)
        {
            var damageDealt = Schaden * Schadensmodifier;
            target.Lebenspunkte -= damageDealt;
            
            return (int)damageDealt; 
        }
    }
}