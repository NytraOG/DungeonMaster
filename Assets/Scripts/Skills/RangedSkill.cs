using System;
using System.Globalization;
using Entities;
using UnityEngine;
using Random = System.Random;

namespace Skills
{
    [CreateAssetMenu(fileName = "Ranged Skill", menuName = "Skills/Ranged")]
    public class RangedSkill : BaseDamageSkill
    {
        public override Factions TargetableFaction => Factions.Foe;

        public override string Activate(BaseUnit actor, BaseUnit target)
        {
            var damage = GetDamage(actor);

            var minhit = damage.Item1;
            var maxhit = damage.Item2;

            var rando         = new Random();
            var damageInRange = rando.NextDouble() * (maxhit - minhit) + minhit;

            target.CurrentHitpoints -= (int)damageInRange;

            var finalDamage = ((int)damageInRange).ToString();

            OnDamageDealt?.Invoke(finalDamage);

            return finalDamage;
        }
    }
}