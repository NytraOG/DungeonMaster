using Entities;
using UnityEngine;

namespace Skills.neu
{
    [CreateAssetMenu(fileName = "Magic Skill", menuName = "Skills/Magic")]
    public class MagicSkill : BaseDamageSkill
    {
        public override void Activate(BaseUnit actor, BaseUnit target) => target.CurrentHitpoints -= GetDamage(actor);
    }
}