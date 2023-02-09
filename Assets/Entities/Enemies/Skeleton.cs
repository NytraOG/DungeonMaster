using Battlefield;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemies
{
    public class Skeleton : BaseFoe
    {
        private         Random        rng;
        private         BattleService service;
        public override float         Schadensmodifier => 1.0f;

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
            Schaden      = 6;

            SetInitialHitpointsAndMana();
            Initialize();
        }

        private void OnMouseDown()
        {
            service.selectedEnemy = gameObject.GetComponent<Skeleton>();

            Debug.Log("Bur");
        }

        public override int DealDamage(BaseUnit target)
        {
            var modifier    = rng.Next(0, 2);
            var damageDealt = Schaden * Schadensmodifier * modifier;
            target.Hitpoints -= damageDealt;

            if (target.Hitpoints < 0)
                target.Hitpoints = 0;

            return (int)damageDealt;
        }
    }
}