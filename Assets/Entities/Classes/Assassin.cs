namespace Entities.Classes
{
    public sealed class Assassin : BaseHero
    {
        public Assassin()
        {
            Strength           = 1;
            Constitution     = 1;
            Dexterity        = 1;
            Quickness       = 1;
            Intuition        = 2;
            Logic            = 1;
            Willpower     = 1;
            Wisdom         = 1;
            Charisma         = 1;
            Hitpoints     = HitpointsMax;
            Schaden          = 1;

            InitiativeBestimmen();
        }

        public override float Schadensmodifier => 1.25f;

        public override void DealDamage(BaseUnit target) => target.Lebenspunkte -= Schaden * Schadensmodifier;
    }
}