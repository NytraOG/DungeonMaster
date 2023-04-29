using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    public class Shiv : BaseAbility
    {
        public override string AbilityName => AbilityNames.Shiv;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override int TriggerAbility(BaseUnit actor = null, BaseUnit target = null)
        {
            if (actor is null || target is null)
                throw new ArgumentNullException(nameof(actor));

            target.Hitpoints -= actor.Schaden;

            return actor.Schaden;
        }
    }
}