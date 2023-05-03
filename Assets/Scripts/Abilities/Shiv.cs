using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Shiv", menuName = "Abilities/Shiv", order = 0)]
    public class Shiv : BaseAbility
    {
        public override string AbilityName => AbilityNames.Shiv;

        public override string GetTooltip(int damage = 0) => $"<b>{AbilityName.ToUpper()}</b>{Environment.NewLine}" +
                                                                        $"<i>Melee, Basic</i>{Environment.NewLine}{Environment.NewLine}" +
                                                                        GetDamageText(damage) +
                                                                        $"A basic stab with a small blade, that can actually deal some harm and {Environment.NewLine}" +
                                                                        $"cause bleeding. It's perfect for close combat and surprise attacks, {Environment.NewLine}" +
                                                                        "but requires precision and timing to use effectively";

        public override float GetDamage(BaseUnit actor) => actor.Dexterity / 2 + actor.Wisdom / 3 + actor.Quickness / 2 - 1;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override int TriggerAbility(BaseUnit actor, BaseUnit target)
        {
            var damageDealt = GetDamage(actor);
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}