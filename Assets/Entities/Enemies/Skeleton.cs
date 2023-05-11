using Skills;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemies
{
    public class Skeleton : BaseFoe
    {
        private         Random rng;
        private void Start()
        {
            rng          = new Random();
            Strength     = 5;
            Constitution = 6;
            Dexterity    = 2;
            Quickness    = 3;
            Intuition    = 2;
            Logic        = 1;
            Willpower    = 1;
            Wisdom       = 1;
            Charisma     = 1;

            SetInitialHitpointsAndMana();
            Initialize();
        }

        private void OnMouseDown() => Debug.Log("Bur");

        public override float GetApproximateDamage(BaseSkill ability) => throw new System.NotImplementedException();

        public override string UseAbility(BaseSkill ability, BaseUnit target = null) => throw new System.NotImplementedException();
    }
}