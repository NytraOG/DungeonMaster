using System.Globalization;
using System.Linq;
using Entities;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Melee Skill", menuName = "Skills/Melee")]
    public class MeleeSkill : BaseDamageSkill
    {
        public override Factions TargetableFaction => Factions.Foe;

        public override string Activate(BaseUnit actor, BaseUnit target)
        {
            var damage = GetDamage(actor);

            var minhit = damage.Item1;
            var maxhit = damage.Item2;

            var rando         = new System.Random();
            var damageInRange = rando.NextDouble() * (maxhit - minhit) + minhit;

            target.CurrentHitpoints -= (int)damageInRange;

            var finalDamage = ((int)damageInRange).ToString();

            OnDamageDealt?.Invoke(finalDamage);

            ApplyDebuffs(actor, target);

            return finalDamage;
        }
    }
}