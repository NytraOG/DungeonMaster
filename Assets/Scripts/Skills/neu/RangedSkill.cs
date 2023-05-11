using Entities;
using UnityEngine;

namespace Skills.neu
{
    [CreateAssetMenu(fileName = "Ranged Skill", menuName = "Skills/Ranged")]
    public class RangedSkill : BaseDamageSkill
    {
        public override void Activate(BaseUnit actor, BaseUnit target) => target.CurrentHitpoints -= GetDamage(actor);
    }
}