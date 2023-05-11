using System;
using Entities;
using Entities.Classes;
using Entities.Races;
using Inventory;
using Skills;
using UI;
using UnityEngine;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        public          Race   race;
        public          BaseClass  classe;
        public          GameObject inventoryPanel;
        public override float      Schadensmodifier => 1.25f;

        private void Awake()
        {
            Strength     = strength;
            Constitution = constitution;
            Dexterity    = dexterity;
            Quickness    = quickness;
            Intuition    = intuition;
            Logic        = logic;
            Willpower    = willpower;
            Wisdom       = wisdom;
            Charisma     = charisma;

            skills.Add(inherentSkill);

            race.ApplyModifiers(this);
            race.ApplyAbilities(this);
            classe.ApplyModifiers(this);
            classe.ApplyAbilities(this);

            SetInitialHitpointsAndMana();

            inventorySystem = new InventorySystem(inventorySize);
        }

        private void OnMouseDown()
        {
            Debug.Log($"{name} clicked");

            var controller = FindObjectOfType<BattleController>();

            if (controller.selectedAbility is SupportSkill { TargetableFaction: Factions.Friend })
                controller.selectedTarget = this;
            else
                ChangeSelectedHero(controller);
        }

        private void ChangeSelectedHero(BattleController controller)
        {
            controller.selectedHero             = this;
            controller.abilitiesOfSelectedHero  = skills;
            controller.abilityanzeigeIstAktuell = false;

            var inventoryDisplay = inventoryPanel.GetComponent<StaticInventoryDisplay>();
            inventoryDisplay.ChangeHero(this);
        }

        public override float GetApproximateDamage(BaseSkill ability) => ability switch
        {
            MagicSkill skill => skill.GetDamage(this),
            MeleeSkill skill => skill.GetDamage(this),
            RangedSkill skill => skill.GetDamage(this),
            SupportSkill _ => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(ability))
        };

        public override string UseAbility(BaseSkill ability, BaseUnit target = null) => ability.Activate(this, target);
    }
}