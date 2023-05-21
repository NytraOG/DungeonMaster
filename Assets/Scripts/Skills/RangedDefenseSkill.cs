using System;
using Entities;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Ranged Defenseskill", menuName = "Skills/Ranged/Defense")]
    public class RangedDefenseSkill : BaseRangedSkill
    {
        public override SkillSubCategory SubCategory       => SkillSubCategory.DefenseSkill;
        public override SkillCategory    Category          => SkillCategory.Ranged;
        public override Factions         TargetableFaction => Factions.Friend;

        public override string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult) => throw new NotImplementedException();
    }
}