using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Shiv", menuName = "Abilities/Shiv", order = 0)]
    public class Shiv : BaseAbility
    {
        public override string AbilityName => AbilityNames.Shiv;
        public override string Tooltip     => $"{AbilityName.ToUpper()}{Environment.NewLine}{Environment.NewLine}" +
                                              $"A basic stab with a small blade, can actually deal some harm and {Environment.NewLine}" +
                                              $"causing bleeding. It's perfect for close combat and surprise attacks, {Environment.NewLine}" +
                                              $"but requires precision and timing to use effectively";

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override int TriggerAbility(BaseUnit actor, BaseUnit target)
        {
            if (actor is null || target is null)
                throw new ArgumentNullException(nameof(actor));

            target.Hitpoints -= actor.Schaden;

            return actor.Schaden;
        }
    }
}