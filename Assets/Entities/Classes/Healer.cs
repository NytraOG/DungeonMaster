using System;

namespace Entities.Classes
{
    public class Healer : BaseHero
    {
        public override float Schadensmodifier => 0.75f;

        public override int DealDamage(BaseUnit target) => throw new NotImplementedException();

        public void InitiativeBestimmen() => throw new NotImplementedException();
    }
}