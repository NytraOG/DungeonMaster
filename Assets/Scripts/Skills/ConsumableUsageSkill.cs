using Entities;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Consumable Usage", menuName = "Skills/Consumable Usage")]
    public  class ConsumableUsageSkill : BaseSkill
    {
        public override Factions TargetableFaction => Factions.All;

        public override string Activate(BaseUnit actor, BaseUnit target) => throw new System.NotImplementedException();
    }
}