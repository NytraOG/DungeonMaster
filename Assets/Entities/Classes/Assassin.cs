namespace Entities.Classes
{
    public sealed class Assassin : BaseHero
    {
        public Assassin()
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
            
            InitiativeBestimmen();
        }

        public override float Schadensmodifier => 1.25f;

        public override void DealDamage(BaseUnit target) => target.Lebenspunkte -= Schaden * Schadensmodifier;
    }
}