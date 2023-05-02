using Abilities;
using Entities.Enums;

namespace Entities.Classes
{
    public abstract class BaseHero : BaseUnit
    {
        public          BaseAbility inherentAbility;
        public override Party       Party => Party.Ally;
    }
}