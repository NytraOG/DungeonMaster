using System;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu]
    public class MagicResistance : BaseAbility
    {
        public override string AbilityName => AbilityNames.MagicResistance;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override void TriggerAbility() => throw new NotImplementedException();
    }
}