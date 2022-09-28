using System;

namespace Entities.Enemies
{
    public class Skeleton : BaseFoe
    {
        public override float Schadensmodifier { get; }

        // Start is called before the first frame update
        private void Start()
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
        }

        // Update is called once per frame
        private void Update() { }

        public override int DealDamage(BaseUnit target) => throw new NotImplementedException();
    }
}