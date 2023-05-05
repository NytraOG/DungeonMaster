using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Fireball", menuName = "Abilities/Fireball", order = 0)]
    public class Firebeam : BaseAbility
    {
        public override string AbilityName => AbilityNames.Firebeam;

        public override string GetTooltip(int damage = 0) => $"<b>{AbilityName.ToUpper()}</b>{Environment.NewLine}" +
                                                             $"<i>Ranged, Magic, Elemental</i>{Environment.NewLine}{Environment.NewLine}" +
                                                             GetDamageText(damage) +
                                                             $"A devastating ability that conjures a sphere of flames, {Environment.NewLine}" +
                                                             $"dealing massive damage to enemies caught in its blast radius.{Environment.NewLine}";

        public override float GetDamage(BaseUnit actor) => actor.Willpower / 2 + actor.Wisdom / 3 + actor.Quickness;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override string TriggerAbility(BaseUnit actor, BaseUnit target)
        {
            var damageDealt = GetDamage(actor);
            target.Hitpoints -= damageDealt;

            return ((int)damageDealt).ToString();
        }
    }
}