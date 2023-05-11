using Entities;
using UnityEngine;

namespace Skills.neu
{
    [CreateAssetMenu(fileName = "Ranged Skill", menuName = "Skills/Ranged")]
    public class RangedSkill : BaseDamageSkill
    {
        public override Factions TargetableFaction => Factions.Foe;

        public override string Activate(BaseUnit actor, BaseUnit target)
        {
            var damageDealt = GetDamage(actor);
            target.CurrentHitpoints -= damageDealt;

            return damageDealt.ToString();
        }
    }
}