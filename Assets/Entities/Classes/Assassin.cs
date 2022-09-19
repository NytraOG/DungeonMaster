namespace Entities.Classes
{
    public sealed class Assassin : BaseHero
    {
        public Assassin()
        {
            Intuition = 2;
            Charisma  = 1;
            Hitpoints = 10;
            Schaden   = 4;

            InitiativeBestimmen(3);
        }

        public override float Schadensmodifier => 1.25f;

        public override int DealDamage(BaseUnit target)
        {
            var damageDealt = Schaden * Schadensmodifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}