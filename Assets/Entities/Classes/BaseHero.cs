using Entities.Enums;

namespace Entities.Classes
{
    public abstract class BaseHero : BaseUnit
    {
        public override Party Party => Party.Ally;
    }
}