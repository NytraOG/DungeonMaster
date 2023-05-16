using System;
using Entities;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Weapon Usage", menuName = "Skills/Weapon Usage")]
    public class WeaponSkill : BaseDamageSkill
    {
        public override Factions TargetableFaction => Factions.Foe;

        public override string Activate(BaseUnit actor, BaseUnit target) => throw new NotImplementedException();
    }
}