using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entities.Buffs
{
    public class BaseBuffable : MonoBehaviour
    {
        // private readonly Dictionary<Guid, TimedBuff> buffs = new();
        //
        // private void Update()
        // {
        //     //OPTIONAL, return before updating each buff if game is paused
        //     //if (Game.isPaused)
        //     //    return;
        //
        //     foreach (var buff in buffs.Values.ToList())
        //     {
        //         buff.Tick(Time.deltaTime);
        //
        //         if (buff.IsFinished)
        //             buffs.Remove(buff.);
        //     }
        // }
        //
        // public void AddBuff(TimedBuff buff)
        // {
        //     if (buffs.ContainsKey(buff.Buff))
        //         buffs[buff.Buff].Activate();
        //     else
        //     {
        //         buffs.Add(buff.Buff, buff);
        //         buff.Activate();
        //     }
        // }
    }
}