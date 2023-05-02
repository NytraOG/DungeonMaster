using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Shiv", menuName = "Abilities/Shiv", order = 0)]
    public class Shiv : BaseAbility
    {
        public override string AbilityName => AbilityNames.Shiv;

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