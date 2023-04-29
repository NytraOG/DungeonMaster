using Abilities;
using Battlefield;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemies
{
    public class Bandit : BaseFoe
    {
        private         Random rng;
        public override float  Schadensmodifier => 1f;

        private void Awake()
        {
            rng              = new Random();
            Intuition        = 2;
            Charisma         = 1;
            MaximumHitpoints = 10;
            Hitpoints        = MaximumHitpoints;
            Schaden          = 3; //TODO ist sinnfrei, dass "Schaden" eine Eigenschaft einer Einheit ist. Gehört nur auf Waffen/Abilities
        }

        private void OnMouseDown()
        {
            var controller = FindObjectOfType<BattleController>();
            controller.selectedEnemy = this;

            Debug.Log("Bur");
        }

        public override int DealDamage(BaseUnit target)
        {
            var modifier    = rng.Next(0, 2);
            var damageDealt = Schaden * Schadensmodifier * modifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }

        public override int UseAbility(BaseAbility ability, BaseUnit target = null) => ability.TriggerAbility(this, target);
    }
}