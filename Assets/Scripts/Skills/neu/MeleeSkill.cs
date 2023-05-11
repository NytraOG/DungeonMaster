using Entities;
using UnityEngine;

namespace Skills.neu
{
    [CreateAssetMenu(fileName = "Melee Skill", menuName = "Skills/Melee")]
    public class MeleeSkill : BaseDamageSkill
    {
        public override void Activate(BaseUnit actor, BaseUnit target) => target.CurrentHitpoints -= GetDamage(actor);
    }
}