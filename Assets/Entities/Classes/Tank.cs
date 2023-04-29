using System;
using Abilities;

namespace Entities.Classes
{
    public class Tank : BaseHero
    {
        public override float Schadensmodifier => 1f;

        public override int DealDamage(BaseUnit target) => throw new NotImplementedException();

        public override int UseAbility(BaseAbility ability, BaseUnit target = null) => throw new NotImplementedException();

        public void InitiativeBestimmen() => throw new NotImplementedException();
    }
}