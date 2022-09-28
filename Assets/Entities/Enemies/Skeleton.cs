using System;

namespace Entities.Enemies
{
    public class Skeleton : BaseFoe
    {
        public override float Schadensmodifier { get; }

        public override void Initialize()
        {
            Strength     = 5;
            Constitution = 6;
            Dexterity    = 2;
            Quickness    = 3;
            Intuition    = 2;
            Logic        = 1;
            Willpower    = 1;
            Wisdom       = 1;
            Charisma     = 1;
            Schaden      = 6;
            
            base.Initialize();
        }

        public override int DealDamage(BaseUnit target) => throw new NotImplementedException();
    }
}