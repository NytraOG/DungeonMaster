using UnityEngine;

namespace Entities.Buffs
{
    [CreateAssetMenu(fileName = "Debuff")]
    public class Debuff : Buff
    {
        public int damagePerTick;

        public void DealDamage(BaseUnit unit) => unit.CurrentHitpoints -= damagePerTick;
    }
}