using Abilities;
using Battlefield;
using UnityEngine;

namespace Entities.Enemies
{
    public class Bandit : BaseFoe
    {
        public override float Schadensmodifier => 1f;

        private void OnMouseDown()
        {
            var controller = FindObjectOfType<BattleController>();
            controller.selectedEnemy = this;

            Debug.Log("Bur");
        }

        public override float GetApproximateDamage(BaseAbility ability) => ability.GetDamage(this);

        public override string UseAbility(BaseAbility ability, BaseUnit target = null)
        {
            var dmg = ability.TriggerAbility(this, target);
            SelectedAbility = null;

            return dmg;
        }
    }
}