using System;
using Battlefield;
using Skills;
using UnityEngine;

namespace Entities.Enemies
{
    public class Bandit : BaseFoe
    {
        public override float Schadensmodifier => 1f;

        private void OnMouseDown()
        {
            var controller = FindObjectOfType<BattleController>();
            controller.selectedTarget = this;

            Debug.Log($"{name} clicked");
        }

        public override float GetApproximateDamage(BaseSkill ability) => ability switch
        {
            MagicSkill skill => skill.GetDamage(this),
            MeleeSkill skill => skill.GetDamage(this),
            RangedSkill skill => skill.GetDamage(this),
            SupportSkill _ => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(ability))
        };

        public override string UseAbility(BaseSkill ability, BaseUnit target = null)
        {
            var dmg = ability.Activate(this, target);
            SelectedSkill = null;

            return dmg;
        }
    }
}