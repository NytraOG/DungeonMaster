using UnityEngine;

namespace Entities.Buffs
{
    public abstract class TimedBuff
    {
        protected readonly GameObject Obj;
        protected          float      Duration;
        protected          int        EffectStacks;
        public             bool       IsFinished;

        public TimedBuff(Buff buff, GameObject obj)
        {
            Buff = buff;
            Obj  = obj;
        }

        public Buff Buff { get; }

        public void Tick(float delta)
        {
            Duration -= delta;

            if (Duration <= 0)
            {
                End();
                IsFinished = true;
            }
        }

        public void Activate()
        {
            if (Buff.IsEffectStacked || Duration <= 0)
            {
                ApplyEffect();
                EffectStacks++;
            }

            if (Buff.IsDurationStacked || Duration <= 0)
                Duration += Buff.Duration;
        }

        protected abstract void ApplyEffect();

        public abstract void End();
    }
}