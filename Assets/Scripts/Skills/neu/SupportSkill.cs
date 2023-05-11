using System;
using Entities;
using UnityEngine;

namespace Skills.neu
{
    [CreateAssetMenu(fileName = "Support Skill", menuName = "Skills/Support")]
    public class SupportSkill : BaseSkill
    {
        public bool   selfcastOnly;
        public bool   isHealing;
        public bool   isBuffing;
        public int    targets;
        public string @operator;
        public float  modifierMeleeDefense;
        public float  modifierRangedDefense;
        public float  modifierMagicDefense;
        public float  modifierSocialDefense;

        private void Awake()
        {
            if (string.IsNullOrWhiteSpace(@operator))
                @operator = "+";
        }

        public override void Activate(BaseUnit actor, BaseUnit target)
        {
            var ultimateTarget = target;

            ultimateTarget.MeleeDefense  = ApplyOperation(ultimateTarget.MeleeDefense, modifierMeleeDefense);
            ultimateTarget.RangedDefense = ApplyOperation(ultimateTarget.RangedDefense, modifierRangedDefense);
            ultimateTarget.MagicDefense  = ApplyOperation(ultimateTarget.MagicDefense, modifierMagicDefense);
            ultimateTarget.SocialDefense = ApplyOperation(ultimateTarget.SocialDefense, modifierSocialDefense);
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