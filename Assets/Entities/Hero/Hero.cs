using System;
using Battlefield;
using Inventory;
using Skills;
using UI;
using UnityEngine;

namespace Entities.Hero
{
    public class Hero : BaseHero
    {
        public Race       race;
        public HeroClass  heroClass;
        public GameObject inventoryPanel;

        protected override void Awake()
        {
            MapProperties();
            InitializeHero();
            name = $"{race.name}_{heroClass.name}";
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

        private void MapProperties()
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
        }

        private void InitializeHero()
        {
            skills.Add(inherentSkill);

            race.ApplyAbilities(this);
            heroClass.ApplySkills(this);

            SetInitialHitpointsAndMana();

            race.ApplyModifiers(this);

            base.Awake();

            heroClass.ApplyModifiers(this);

            inventorySystem = new InventorySystem(inventorySize);
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