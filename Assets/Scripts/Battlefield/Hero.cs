using System;
using Abilities;
using Entities;
using Entities.Classes;
using Entities.Races;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        public          BaseRace race;
        private         Random   rng;
        public override float    Schadensmodifier => 1.25f;

        private void Awake()
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

            SetInitialHitpointsAndMana();

            abilities.Add(inherentAbility);

            race.ApplyModifiers(this);
            race.ApplyAbilities(this);
        }

        private void OnMouseDown()
        {
            var controller = FindObjectOfType<BattleController>();

            controller.selectedHero             = this;
            controller.abilitiesOfSelectedHero  = abilities;
            controller.abilityanzeigeIstAktuell = false;

            Debug.Log("kek");
        }

        public override float GetApproximateDamage(BaseAbility ability) => ability.GetDamage(this);

        public override int? UseAbility(BaseAbility ability, BaseUnit target = null) => ability.TriggerAbility(this, target);
    }
}