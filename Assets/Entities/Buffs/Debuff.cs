using UnityEngine;

namespace Entities.Buffs
{
    [CreateAssetMenu(fileName = "Debuff")]
    public class Debuff : Buff
    {
        public int damagePerTick;

        public override string ResolveTick(BaseUnit applicant)
        {
            DealDamage(applicant);
            remainingDuration--;

            return damagePerTick.ToString();
        }

        public override void Die(BaseUnit applicant)
        {
            Reverse();
            applicant.debuffs.Remove(this);
            Destroy(this);
        }

        private void DealDamage(BaseUnit unit) => unit.CurrentHitpoints -= damagePerTick;
    }
}