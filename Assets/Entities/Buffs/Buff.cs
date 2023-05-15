using Skills;
using UnityEngine;

namespace Entities.Buffs
{
    [CreateAssetMenu(fileName = "Buff")]
    public class Buff : BaseUnitModifikator
    {
        public int       duration = 1;
        public int       currentDuration;
        public BaseSkill appliedBy;
        public BaseUnit  appliedFrom;
        public bool      DurationEnded => currentDuration <= 0;

        private void Awake() => currentDuration = duration;
        public void Reverse(){}
    }
}