using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Magic Resistance", menuName = "Abilities/Magic Resistance", order = 0)]
    public class MagicResistance : BaseAbility
    {
        public override string AbilityName => AbilityNames.MagicResistance;

        public override string GetTooltip(int damage = 0) => $"<b>{AbilityName.ToUpper()}</b>{Environment.NewLine}" +
                                                                        $"<i>Defence, Magic</i>{Environment.NewLine}{Environment.NewLine}" +
                                                                        GetDamageText(damage) +
                                                                        $"Through hard mental training, the character is able to fend off enemy {Environment.NewLine}" +
                                                                        $"magical attacks with his sheer willpower.{Environment.NewLine}";

        public override float GetDamage(BaseUnit actor) => 0;

        public override void Initialize(GameObject obj) => throw new NotImplementedException();

        public override int TriggerAbility(BaseUnit actor, BaseUnit target)
        {
            actor.MagicDefense += 10;

            return 0;
        }
    }
}