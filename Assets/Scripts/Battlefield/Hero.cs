using Entities;
using Entities.Classes;
using Entities.Races;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        public          BaseRace      race;
        private         Random        rng;
        private         BattleService service;
        public override float         Schadensmodifier => 1.25f;

        private void Start()
        {
            Strength     = 5;
            Constitution = 3;
            Dexterity    = 9;
            Quickness    = 8;
            Intuition    = 8;
            Logic        = 5;
            Willpower    = 2;
            Wisdom       = 5;
            Charisma     = 1;
            Schaden      = 10;
            rng          = new Random();

            service      = battleService.GetComponent<BattleService>();

            SetInitialHitpointsAndMana();
            race.ApplyModifiers(this);
            race.ApplyAbilities(this);
        }

        private void OnMouseDown()
        {
            Debug.Log("kek");

            service.selectedHero = gameObject.GetComponent<Hero>() ;
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