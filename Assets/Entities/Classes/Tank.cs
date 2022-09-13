using System;

namespace Entities.Classes
{
    public class Tank : BaseHero
    {
        public override float Schadensmodifier => 1f;

        public override void DealDamage(BaseUnit target) => throw new NotImplementedException();

        public override void InitiativeBestimmen() => throw new NotImplementedException();
    }
}