using System;
using Entities;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Consumable Usage", menuName = "Skills/Consumable Usage")]
    public class ConsumableUsageSkill : BaseSkill
    {
        public override string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult) => throw new NotImplementedException();
    }
}