using System;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Magic Resistance", menuName = "Abilities/Magic Resistance", order = 0)]
    public class MagicResistance : BaseAbility
    {
        public override string AbilityName => AbilityNames.MagicResistance;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override void TriggerAbility() => throw new NotImplementedException();
    }
}