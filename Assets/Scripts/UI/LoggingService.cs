using System;
using System.Collections.Generic;
using System.Linq;
using Battlefield;
using Entities;
using Entities.Enums;
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
        public           GameObject       spawnController;
        public           int              fontsize;
        private readonly List<GameObject> logTracker      = new();
        private readonly int              maxMessagecount = 30;
        private          string           StunInfo => " <b><color=yellow>Stun</color></b> applied";

        public void Awake()
        {
            var bController = battleController.GetComponent<BattleController>();
            var sController = spawnController.GetComponent<SpawnController>();

            bController.OnHit           += OnHit;
            bController.OnMiss          += OnMiss;
            bController.OnMisc          += OnMisc;
            bController.OnBuffApplied   += OnBuffApplied;
            bController.OnDebuffTick    += OnDebuffTick;
            bController.OnBuffTick      += OnBuffTick;
            sController.OnCreateSpawned += OnCreateSpawned;
        }

        private void OnBuffTick(BuffResolutionArgs args)
        {
            if (args.RemainingDuration < 0)
                return;

            if (args.RemainingDuration == 0)
            {
                Log($"<b><color=#{ColorUtility.ToHtmlStringRGBA(args.CombatlogEffectColor)}>{args.Buff.name}</color></b> " +
                    $"expired on {FetchUnitnameWithMatchingColor(args.Applicant)}.");
            }
            else
            {
                Log($"<b><color=#{ColorUtility.ToHtmlStringRGBA(args.CombatlogEffectColor)}>{args.Buff.name}</color></b> " +
                    $"{args.RemainingDuration} turns remaining on {FetchUnitnameWithMatchingColor(args.Applicant)}.");
            }
        }

        private void OnCreateSpawned(SpawnController.SpawnEventArgs args) => Log($"{FetchUnitnameWithMatchingColor(args.Creature)} level {args.Creature.level} appeared at position <b>{GetPositionString(args.Position)}</b>.");

        private void OnDebuffTick(DebuffResolutionArgs args)
        {
            if (args.RemainingDuration < 0)
                return;

            if (args.Debuff.damagePerTick != 0)
            {
                Log($"{FetchUnitnameWithMatchingColor(args.Actor)} lost {FetchDamageText(args.Damage)} Health " +
                    $"to <b><color=#{ColorUtility.ToHtmlStringRGBA(args.CombatlogEffectColor)}>{args.Debuff.name}</color></b>, " +
                    $"{args.RemainingDuration} turns remaining.");
            }
            else
            {
                if (args.RemainingDuration == 0)
                {
                    Log($"<b><color=#{ColorUtility.ToHtmlStringRGBA(args.CombatlogEffectColor)}>{args.Debuff.name}</color></b> " +
                        $"expired on {FetchUnitnameWithMatchingColor(args.Actor)}.");
                }
                else
                {
                    Log($"<b><color=#{ColorUtility.ToHtmlStringRGBA(args.CombatlogEffectColor)}>{args.Debuff.name}</color></b> " +
                        $"{args.RemainingDuration} turns remaining on {FetchUnitnameWithMatchingColor(args.Actor)}.");
                }
            }
        }

        private void OnHit(CombatskillResolutionArgs args)
        {
            var content = $"{FetchUnitnameWithMatchingColor(args.Actor)}'s[{(int)args.Actor.ModifiedInitiative}] {args.Skill.name} " +
                          $"hit[{args.Hitroll}] {FetchUnitnameWithMatchingColor(args.Target)}[{FetchDefenseattribute(args)}] " +
                          $"for {FetchDamageText(args)} damage.";

            if (args.Target.IsStunned)
                content += StunInfo;

            if (args.Target.IsDead)
                content += " <b><color=red>FATAL!</color></b>";

            Log(content);
        }

        private void OnMiss(CombatskillResolutionArgs args)
        {
            var content = $"{FetchUnitnameWithMatchingColor(args.Actor)}'s[{(int)args.Actor.ModifiedInitiative}] {args.Skill.name} " +
                          $"missed[{args.Hitroll}] {FetchUnitnameWithMatchingColor(args.Target)}[{FetchDefenseattribute(args)}].";

            Log(content);
        }

        private void OnBuffApplied(BaseUnit actor, BaseSkill skill, List<BaseUnit> targets, string _)
        {
            var content = $"{FetchUnitnameWithMatchingColor(actor)}[{(int)actor.ModifiedInitiative}] used {skill.name} " +
                          $"on {string.Join(", ", targets.Select(FetchUnitnameWithMatchingColor))}.";

            if (targets.Any(t => t.IsStunned))
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

        private string FetchUnitnameWithMatchingColor(BaseUnit unit) => $"<b><color={unit.CombatLogColor}>{unit.name}</color></b>";

        private string FetchDamageText(int damage) => $"<b><color={Konstanten.NormalDamageColor}>{damage}</color></b>";

        private string FetchDamageText(CombatskillResolutionArgs args) => args.HitResult switch
        {
            HitResult.None => $"<color={Konstanten.NormalDamageColor}>{args.Abilityresult}</color>",
            HitResult.Normal => $"<b><color={Konstanten.NormalDamageColor}>{args.Abilityresult}</color></b>",
            HitResult.Good => $"<b><color={Konstanten.GoodDamageColor}>{args.Abilityresult}</color></b>",
            HitResult.Critical => $"<b><color={Konstanten.CriticalDamageColor}>*{args.Abilityresult}*</color></b>",
            _ => $"<color={Konstanten.NormalDamageColor}>{args.Abilityresult}</color>"
        };

        private string GetPositionString(Positions position) => position switch
        {
            Positions.None => "N/A",
            Positions.FrontMiddel => "Front",
            Positions.FrontLeft => "Front",
            Positions.FrontRight => "Front",
            Positions.LeftFlankMiddel => "Left flank",
            Positions.LeftFlankLeft => "Left flank",
            Positions.LeftFlankright => "Left flank",
            Positions.RightFlankMiddle => "Right flank",
            Positions.RightFlankLeft => "Right flank",
            Positions.RightFlankRight => "Right flank",
            Positions.CenterMiddle => "Center",
            Positions.CenterLeft => "Center",
            Positions.CenterRight => "Center",
            Positions.BackMiddle => "Back",
            Positions.BackLeft => "Back",
            Positions.BackRight => "Back",
            Positions.All => "N/A",
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
        };

        private int FetchDefenseattribute(CombatskillResolutionArgs info) => info.Skill.category switch
        {
            SkillCategory.Melee => (int)info.Target.ModifiedMeleeDefense,
            SkillCategory.Ranged => (int)info.Target.ModifiedRangedDefense,
            SkillCategory.Magic => (int)info.Target.ModifiedMagicDefense,
            SkillCategory.Social => (int)info.Target.ModifiedSocialDefense,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}