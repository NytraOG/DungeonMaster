using Entities;
using Entities.Enums;
using UnityEngine;
using Random = System.Random;

namespace Skills
{
    [CreateAssetMenu(fileName = "Magic Skill", menuName = "Skills/Magic")]
    public abstract class BaseMagicSkill : BaseDamageSkill
    {
        public override Factions TargetableFaction => Factions.Foe;

        public override string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult)
        {
            var damage = GetDamage(actor, hitResult);

            var minhit = damage.Item1;
            var maxhit = damage.Item2;

            var rando         = new Random();
            var damageInRange = rando.NextDouble() * (maxhit - minhit) + minhit;

            target.CurrentHitpoints -= (int)damageInRange;

            var finalDamage = ((int)damageInRange).ToString();

            OnDamageDealt?.Invoke(finalDamage);

            ApplyDebuffs(actor, target);

            return finalDamage;
        }
    }
}