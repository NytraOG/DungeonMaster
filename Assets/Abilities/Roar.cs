using System;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu]
    public class Roar : BaseAbility
    {
        public override string AbilityName => AbilityNames.Roar;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override void TriggerAbility() => throw new NotImplementedException();
    }
}