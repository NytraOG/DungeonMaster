using System;
using Abilities;
using Entities;
using Entities.Classes;
using Entities.Races;
using UnityEngine;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        public          BaseRace  race;
        public          BaseClass classe;
        public override float     Schadensmodifier => 1.25f;

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

            abilities.Add(inherentAbility);

            race.ApplyModifiers(this);
            race.ApplyAbilities(this);
            classe.ApplyModifiers(this);
            classe.ApplyAbilities(this);

            SetInitialHitpointsAndMana();
        }

        private void OnMouseEnter()
        {
            var asd = GetComponent<SpriteRenderer>();
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

        public override string UseAbility(BaseAbility ability, BaseUnit target = null) => ability.TriggerAbility(this, target);
    }
}