using Entities;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Magic Defenseskill", menuName = "Skills/Magic/Defense")]
    public class MagicDefenseSkill : BaseMagicSkill
    {
        public override SkillSubCategory SubCategory => SkillSubCategory.DefenseSkill;
        public override SkillCategory    Category    => SkillCategory.Magic;

        public override string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult) => throw new System.NotImplementedException();

        public override Factions TargetableFaction => Factions.Friend;
    }
}