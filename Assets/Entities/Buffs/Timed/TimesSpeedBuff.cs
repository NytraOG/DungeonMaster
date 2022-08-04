using Entities.Buffs.Scriptable;
using UnityEngine;

namespace Entities.Buffs.Timed
{
    public class TimesSpeedBuff : TimedBuff
    {
        private readonly Component movementComponent;

        public TimesSpeedBuff(Buff buff, GameObject obj) : base(buff, obj) => movementComponent = obj.GetComponent("");

        protected override void ApplyEffect()
        {
            if (Buff is not SpeedBuff speedBuff)
                return;

            //movementComponent.MovementSpeed += speedBuff.speedIncrease;
        }

        public override void End()
        {
            if (Buff is not SpeedBuff speedBuff)
                return;

            // movementComponent.MovementSpeed -= speedBuff.speedIncrease * EffectStacks;
            // EffectStacks                    =  0;
        }
    }
}