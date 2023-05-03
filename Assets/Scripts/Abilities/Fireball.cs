using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Fireball", menuName = "Abilities/Fireball", order = 0)]
    public class Fireball : BaseAbility
    {
        public override string AbilityName => AbilityNames.Fireball;
        public override string Tooltip     => $"{AbilityName.ToUpper()}{Environment.NewLine}{Environment.NewLine}" +
                                              $"A devastating ability that conjures a sphere of flames, {Environment.NewLine}" +
                                              $"dealing massive damage to enemies caught in its blast radius.{Environment.NewLine}";

        public override void Initialize(GameObject obj) => throw new System.NotImplementedException();

        public override int TriggerAbility(BaseUnit actor, BaseUnit target) => throw new System.NotImplementedException();
    }
}