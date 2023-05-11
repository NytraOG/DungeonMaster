using Entities;
using UnityEngine;

namespace Skills.neu
{
    [CreateAssetMenu(fileName = "Melee Skill", menuName = "Skills/Melee")]
    public class MeleeSkill : BaseDamageSkill
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