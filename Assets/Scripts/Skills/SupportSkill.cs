using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Buffs;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Support Skill", menuName = "Skills/Support")]
    public class SupportSkill : BaseTargetingSkill
    {
        public                                    bool       selfcastOnly;
        public                                    bool       isHealing;
        public                                    bool       isBuffing;
        public                                    bool       targetsWholeGroup;
        [Header("Attribute Modification")] public string     @operator = "+";
        public                                    float      modifierMeleeDefense;
        public                                    float      modifierRangedDefense;
        public                                    float      modifierMagicDefense;
        public                                    float      modifierSocialDefense;
        public                                    List<Buff> appliedBuffs = new();
        public override                           Factions   TargetableFaction => Factions.Friend;

        private void Awake()
        {
            if (string.IsNullOrWhiteSpace(@operator))
                @operator = "+";
        }

        public override string Activate(BaseUnit actor, BaseUnit target, HitResult _)
        {
            if (isBuffing)
                ApplyBuffs(actor, target);

            var ultimateTarget = target;

            ultimateTarget.IsStunned = appliesStun;

            ultimateTarget.MeleeDefense  = ApplyOperation(ultimateTarget.MeleeDefense, modifierMeleeDefense);
            ultimateTarget.RangedDefense = ApplyOperation(ultimateTarget.RangedDefense, modifierRangedDefense);
            ultimateTarget.MagicDefense  = ApplyOperation(ultimateTarget.MagicDefense, modifierMagicDefense);
            ultimateTarget.SocialDefense = ApplyOperation(ultimateTarget.SocialDefense, modifierSocialDefense);

            return appliesStun ? "STUN" : "BUFFED";
        }

        private void ApplyBuffs(BaseUnit actor, BaseUnit target)
        {
            foreach (var buff in appliedBuffs)
            {
                if (target.buffs.Any(b => b.displayname == buff.displayname))
                    target.buffs.First(b => b.displayname == buff.displayname).remainingDuration += buff.duration;
                else
                {
                    var newBuffInstance = buff.ToNewInstance();

                    newBuffInstance.ApplyAttributeModifier(target);
                    newBuffInstance.ApplyRatingModifier(target);
                    newBuffInstance.ApplyDamageModifier(target);
                    newBuffInstance.appliedBy   = this;
                    newBuffInstance.appliedFrom = actor;

                    target.buffs.Add(newBuffInstance);
                }
            }
        }

        private float ApplyOperation(float attributeValue, float modifier) => @operator switch
        {
            "+" => attributeValue + modifier,
            "-" => attributeValue - modifier,
            "*" => attributeValue * modifier,
            "/" => attributeValue / modifier,
            _ => throw new Exception("Ins Operatorfield kommen nur +, -, *, / rein >:C")
        };
    }
}