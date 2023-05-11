using Entities;
using UnityEngine;

namespace Skills.neu
{
    [CreateAssetMenu(fileName = "Magic Skill", menuName = "Skills/Magic")]
    public class MagicSkill : BaseDamageSkill
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