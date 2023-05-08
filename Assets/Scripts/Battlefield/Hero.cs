using Abilities;
using Entities;
using Entities.Classes;
using Entities.Races;
using Inventory;
using UI;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        public          BaseRace  race;
        public          BaseClass classe;
        public override float     Schadensmodifier => 1.25f;

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

            abilities.Add(inherentAbility);

            race.ApplyModifiers(this);
            race.ApplyAbilities(this);
            classe.ApplyModifiers(this);
            classe.ApplyAbilities(this);

            SetInitialHitpointsAndMana();

            inventorySystem = new InventorySystem(inventorySize);
        }

        private void OnMouseDown()
        {
            var controller = FindObjectOfType<BattleController>();

            controller.selectedHero             = this;
            controller.abilitiesOfSelectedHero  = abilities;
            controller.abilityanzeigeIstAktuell = false;

            var inventoryDisplay = FindObjectOfType<StaticInventoryDisplay>();
            inventoryDisplay.ChangeHero(this);
        }

        public override float GetApproximateDamage(BaseAbility ability) => ability.GetDamage(this);

        public override string UseAbility(BaseAbility ability, BaseUnit target = null) => ability.TriggerAbility(this, target);
    }
}