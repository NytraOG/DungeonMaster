namespace Entities.Enemies
{
    public sealed class Goblin : BaseFoe
    {
        public Goblin()
        {
            Intuition    = 1;
            Charisma     = 1;
            Lebenspunkte = 10;
            Schaden      = 4;

            InitiativeBestimmen(2);
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