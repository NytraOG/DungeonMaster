using Entities.Enums;

namespace Entities.Enemies
{
    public abstract class BaseFoe : BaseUnit
    {
        public override Party Party => Party.Foe;
    }
}