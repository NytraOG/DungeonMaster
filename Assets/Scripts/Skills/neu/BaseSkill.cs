using System;
using System.Text;
using Entities;
using UnityEngine;

namespace Skills.neu
{
    public abstract class BaseSkill : ScriptableObject
    {
        public                  Sprite   sprite;
        public                  string   displayName;
        [TextArea(4, 4)] public string   description;
        public                  float    scalingStrength;
        public                  float    scalingConstitution;
        public                  float    scalingDexterity;
        public                  float    scalingQuickness;
        public                  float    scalingIntuition;
        public                  float    scalingLogic;
        public                  float    scalingWillpower;
        public                  float    scalingWisdom;
        public                  float    scalingCharisma;
        public                  int      addedFlatDamage;
        public                  bool     appliesStun;
        public                  string[] keywords;

        private string Description
        {
            get
            {
                var input             = description;
                var lineBreakInterval = 60;

                var emil = new StringBuilder();

                for (var i = 0; i < input.Length; i += lineBreakInterval)
                {
                    var remainingLength = Math.Min(lineBreakInterval, input.Length - i);
                    var segment         = input.Substring(i, remainingLength);
                    emil.Append(segment);

                    if (i + remainingLength < input.Length)
                        emil.AppendLine();
                }

                return emil.ToString();
            }
        }

        public abstract Factions TargetableFaction { get; }

        public abstract string Activate(BaseUnit actor, BaseUnit target);

        public string GetTooltip(int damage = 0) => $"<b>{displayName.ToUpper()}</b>{Environment.NewLine}" +
                                                    $"<i>{string.Join(", ", keywords)}</i>{Environment.NewLine}{Environment.NewLine}" +
                                                    GetDamageText(damage) +
                                                    Description;

        private string GetDamageText(int damage) => damage == 0 ? string.Empty : $"Damage: <b>{damage}</b>{Environment.NewLine}{Environment.NewLine}";
    }
}