using System;
using System.Collections.Generic;
using System.Linq;
using Battlefield;
using Entities;
using Skills;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LoggingService : MonoBehaviour
    {
        public           GameObject       combatLog;
        public           GameObject       logmessagePrefab;
        public           GameObject       battleController;
        public           int              fontsize;
        private readonly List<GameObject> logTracker      = new();
        private readonly int              maxMessagecount = 30;
        private          string           StunInfo => " <b><color=yellow>Stun</color></b> applied";

        public void Awake()
        {
            var component = battleController.GetComponent<BattleController>();

            component.OnHit         += OnHit;
            component.OnMiss        += OnMiss;
            component.OnMisc        += OnMisc;
            component.OnBuffApplied += OnBuffApplied;
        }

        private void OnHit(CombatskillResolutionArgs args)
        {
            var content = $"{args.Actor.name}'s[{(int)args.Actor.CurrentInitiative}] {args.Skill.name} " +
                          $"hit[{args.Hitroll}] {args.Target.name}[{FetchDefenseattribute(args)}] " +
                          $"for {args.Abilityresult} damage.";

            if (args.Target.IsStunned)
                content += StunInfo;

            if (args.Target.IsDead)
                content += " <b><color=red>FATAL!</color></b>";

            Log(content);
        }

        private void OnMiss(CombatskillResolutionArgs args)
        {
            var content = $"{args.Actor.name}'s[{(int)args.Actor.CurrentInitiative}] {args.Skill.name} " +
                          $"missed[{args.Hitroll}] {args.Target.name}[{FetchDefenseattribute(args)}].";

            Log(content);
        }

        private void OnBuffApplied(BaseUnit actor, BaseSkill skill, BaseUnit target, string abilityResult)
        {
            var content = $"{actor.name}[{(int)actor.CurrentInitiative}] used {skill.name} on {target.name}.";

            if (target.IsStunned)
                content += StunInfo;

            Log(content);
        }

        private void OnMisc(string message) => Log(message);

        private void Log(string message)
        {
            if (logTracker.Count >= maxMessagecount)
            {
                var entry = logTracker[0];
                logTracker.Remove(entry);
                Destroy(entry);
            }

            var logEntry   = Instantiate(logmessagePrefab, combatLog.transform);
            var textObject = logEntry.GetComponent<TextMeshProUGUI>();
            textObject.margin   = new Vector4(5, 0, 0, 5);
            textObject.fontSize = fontsize;
            textObject.text     = message;

            if (logTracker.Any())
            {
                logTracker[^1]
                       .GetComponent<TextMeshProUGUI>()
                       .margin = new Vector4(5, 0, 0, 2);
            }

            logTracker.Add(logEntry);
        }

        private int FetchDefenseattribute(CombatskillResolutionArgs info) => info.Skill switch
        {
            MagicSkill => (int)info.Target.MagicDefense,
            MeleeSkill => (int)info.Target.MeleeDefense,
            RangedSkill => (int)info.Target.RangedDefense,
            WeaponSkill => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}