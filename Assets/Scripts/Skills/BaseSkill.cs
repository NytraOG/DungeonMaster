using System;
using System.Text;
using Entities;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    public abstract class BaseSkill : ScriptableObject
    {
        public                            Sprite   sprite;
        public                            string   displayName;
        public                            bool     appliesStun;
        public                            string[] keywords;
        [TextArea(4, 4)] public           string   description;
        public                            int      xpDeltaPlusRoot;
        [Header("Effect Scaling")] public float    dStrength;
        public                            float    dConstitution;
        public                            float    dDexterity;
        public                            float    dQuickness;
        public                            float    dIntuition;
        public                            float    dLogic;
        public                            float    dWillpower;
        public                            float    dWisdom;
        public                            float    dCharisma;
        public                            int      addedFlatDamage;
        public                            int      level = 1;
        public                            int      manacost;
        [Header("0 bis 1")] public        float    damageRange;

        private string Description
        {
            get
            {
                var input             = description;
                var lineBreakInterval = 60;

                var lineBreak = Environment.NewLine;

                var emil  = new StringBuilder();
                var count = 0;

                for (var i = 0; i < input.Length; i++)
                {
                    emil.Append(input[i]);
                    count++;

                    if (count < lineBreakInterval)
                        continue;

                    if (char.IsWhiteSpace(input[i]) && i < input.Length - 1 && !char.IsWhiteSpace(input[i + 1]))
                    {
                        emil.Append(lineBreak);
                        count = 0;
                    }
                    else
                    {
                        var index = i;

                        while (index > 0 && !char.IsWhiteSpace(input[index]))
                            index--;

                        if (index > 0)
                        {
                            emil.Insert(index + 1, lineBreak);
                            count = i - (index + 1);
                        }
                        else
                        {
                            emil.Append(lineBreak);
                            count = 0;
                        }
                    }
                }

                return emil.ToString();
            }
        }

        public int UpgradeCostXp   => GetXpTotal(level + 1);
        public int UpgradeCostGold => GetGoldTotal(level + 1);

        public abstract string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult);

        public string GetTooltip(string damage = "0-0") => $"<b>{displayName.ToUpper()}</b>{Environment.NewLine}" +
                                                           $"<i>{string.Join(", ", keywords)}</i>{Environment.NewLine}{Environment.NewLine}" +
                                                           GetDamageText(damage) +
                                                           Description;

        private int GetXpDelta(int inputLevel)
        {
            if (inputLevel == 0)
                return inputLevel;

            var xpDelta = inputLevel * xpDeltaPlusRoot + GetXpDelta(inputLevel - 1);

            return xpDelta;
        }

        private int GetXpTotal(int inputLevel)
        {
            if (inputLevel == 0)
                return inputLevel;

            var xpTotal = GetXpDelta(inputLevel) + GetXpTotal(inputLevel - 1);

            return xpTotal;
        }

        private int GetGoldDelta(int inputLevel) => (int)(GetXpDelta(inputLevel) * 0.9);

        private int GetGoldTotal(int inputLevel)
        {
            if (inputLevel == 0)
                return inputLevel;

            var goldTotal = GetGoldDelta(inputLevel) + GetGoldTotal(inputLevel - 1);

            return goldTotal;
        }

        private string GetDamageText(string damage) => damage == "0-0" ? string.Empty : $"Damage: <b>{damage}</b>{Environment.NewLine}{Environment.NewLine}";
    }
}