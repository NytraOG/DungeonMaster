using System;
using Entities;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Consumable Usage", menuName = "Skills/Consumable Usage")]
    public class ConsumableUsageSkill : BaseSkill
    {
        private void Awake()
        {
            category    = SkillCategory.Support;
            subCategory = SkillSubCategory.Special;
        }

        public override string Activate(BaseUnit actor) => throw new NotImplementedException();
    }
}