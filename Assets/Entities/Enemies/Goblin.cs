using Skills;

namespace Entities.Enemies
{
    public sealed class Goblin : BaseFoe
    {
        public Goblin()
        {
            Intuition = 1;
            Charisma  = 1;

            CurrentHitpoints = 10;

            InitiativeBestimmen(2);
        }

        public override float GetApproximateDamage(BaseSkill ability) => throw new System.NotImplementedException();

        public override string UseAbility(BaseSkill ability, BaseUnit target = null) => throw new System.NotImplementedException();
    }
}