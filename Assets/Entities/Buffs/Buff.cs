using UnityEngine;

namespace Entities.Buffs
{
    public abstract class Buff : ScriptableObject
    {
        public BaseUnit Target            { get; set; }
        public float    Duration          { get; set; }
        public bool     IsEffectStacked   { get; set; }
        public bool     IsDurationStacked { get; set; }

        public abstract TimedBuff InitializeBuff(GameObject obj);
    }
}