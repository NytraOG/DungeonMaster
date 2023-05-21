using Entities;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Melee Defenseskill", menuName = "Skills/Melee/Defense")]
    public class MeleeDefenseSkill : BaseMeleeSkill
    {
        public override SkillSubCategory SubCategory => SkillSubCategory.DefenseSkill;
        public override SkillCategory    Category    => SkillCategory.Melee;

        public override string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult) => throw new System.NotImplementedException();

        public override Factions TargetableFaction => Factions.Friend;
    }
}