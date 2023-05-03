using System;
using Abilities;
using Battlefield;
using UnityEngine;

namespace Entities.Enemies
{
    public class Bandit : BaseFoe
    {
        public override float Schadensmodifier => 1f;

        private void Awake()
        {
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

        public override float GetApproximateDamage(BaseAbility ability) => ability.GetDamage(this);

        public override int? UseAbility(BaseAbility ability, BaseUnit target = null)
        {
            var dmg = ability.TriggerAbility(this, target);
            SelectedAbility = null;
            return dmg;
        }
    }
}