using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Roar", menuName = "Abilities/Roar", order = 0)]
    public class Roar : BaseAbility
    {
        public override string AbilityName => AbilityNames.Roar;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override int TriggerAbility(BaseUnit actor, BaseUnit target)
        {
            var damageDealt = 5;
            target.Hitpoints -= damageDealt;

            return damageDealt;
        }
    }
}