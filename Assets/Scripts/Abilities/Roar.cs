using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Roar", menuName = "Abilities/Roar", order = 0)]
    public class Roar : BaseAbility
    {
        public override string AbilityName => AbilityNames.Roar;

        public override string Tooltip => $"{AbilityName.ToUpper()}{Environment.NewLine}{Environment.NewLine}" +
                                          $"A fierce ability that allows a warrior to unleash a deafening scream, {Environment.NewLine}" +
                                          $"stunning nearby enemies and causing them to temporarily lose their composure. {Environment.NewLine}" +
                                          $"This can create an opening for the warrior to strike, or allow for a strategic {Environment.NewLine}" +
                                          $"retreat.";

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override int TriggerAbility(BaseUnit actor, BaseUnit target)
        {
            var damageDealt = 5;
            target.Hitpoints -= damageDealt;

            return damageDealt;
        }
    }
}