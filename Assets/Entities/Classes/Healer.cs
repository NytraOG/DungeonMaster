namespace Entities.Classes
{
    public class Healer : BaseHero
    {
        public override float Schadensmodifier => 0.75f;

        public override void DealDamage(BaseUnit target) => throw new System.NotImplementedException();

        public override void InitiativeBestimmen() => throw new System.NotImplementedException();
    }
}