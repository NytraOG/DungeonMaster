using System;

namespace Entities.Classes
{
    public class Tank : BaseHero
    {
        public override float Schadensmodifier => 1f;

        public override int DealDamage(BaseUnit target) => throw new NotImplementedException();

        public void InitiativeBestimmen() => throw new NotImplementedException();
    }
}