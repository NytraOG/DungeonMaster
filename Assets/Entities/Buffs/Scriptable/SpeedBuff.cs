using System;
using UnityEngine;

namespace Entities.Buffs.Scriptable
{
    [CreateAssetMenu(menuName = "Buffs/SpeedBuff")]
    public class SpeedBuff : Buff
    {
        public float speedIncrease;

        public override TimedBuff InitializeBuff(GameObject obj) => throw new NotImplementedException();
    }
}