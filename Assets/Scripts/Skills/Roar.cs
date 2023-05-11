using System;
using Entities;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Roar", menuName = "Abilities/Roar", order = 0)]
    public class Roar : BaseAbility
    {
        public override string AbilityName => Skillnames.Roar;

        public override string GetTooltip(int damage = 0) => $"<b>{AbilityName.ToUpper()}</b>{Environment.NewLine}" +
                                                                        $"<i>Midrange, Support</i>{Environment.NewLine}{Environment.NewLine}" +
                                                                        GetDamageText(damage) +
                                                                        $"A fierce ability that allows a warrior to unleash a deafening scream, {Environment.NewLine}" +
                                                                        $"stunning nearby enemies and causing them to temporarily lose their composure. {Environment.NewLine}" +
                                                                        $"This can create an opening for the warrior to strike, or allow for a strategic {Environment.NewLine}" +
                                                                        "retreat.";

        public override float GetDamage(BaseUnit actor) => 0;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override string TriggerAbility(BaseUnit actor, BaseUnit target)
        {
            target.IsStunned = true;

            return "STUN";
        }
    }
}