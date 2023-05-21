using Skills;
using UnityEngine;

namespace Entities.Buffs
{
    [CreateAssetMenu(fileName = "Buff")]
    public class Buff : BaseUnitModifikator
    {
        public int       duration;
        public int       remainingDuration;
        public BaseSkill appliedBy;
        public BaseUnit  appliedFrom;
        public bool      isStackable;
        public Color     combatlogEffectColor = Color.white;
        public bool      DurationEnded => remainingDuration == 0;

        private void Awake() => remainingDuration = duration;

        public virtual string ResolveTick(BaseUnit applicant)
        {
            remainingDuration--;

            return $"{name} ticked";
        }

        public virtual void Die(BaseUnit applicant)
        {
            applicant.buffs.Remove(this);
            Destroy(this);
        }

        public virtual void Reverse() { }
    }
}