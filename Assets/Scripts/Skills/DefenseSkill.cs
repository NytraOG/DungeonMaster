using Entities;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Defenseskill", menuName = "Skills/Defense")]
    public class DefenseSkill : BaseSkill
    {
        protected void Awake() => subCategory = SkillSubCategory.Defense;

        public override string Activate(BaseUnit actor) => GetTacticalRoll(actor).ToString();
    }
}