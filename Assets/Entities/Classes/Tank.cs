namespace Entities.Classes
{
    public class Tank : BaseHero
    {
        public override float Schadensmodifier => 1f;

        public override void DealDamage(BaseUnit target) => throw new System.NotImplementedException();

        public override void InitiativeBestimmen() => throw new System.NotImplementedException();
    }
}