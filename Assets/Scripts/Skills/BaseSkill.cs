using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Enums;
using Entities.Hero;
using UnityEngine;

namespace Skills
{
    public abstract class BaseSkill : ScriptableObject
    {
        public                  int              acquisitionLevelBasic      = 1;
        public                  int              acquisitionLevelDemanding  = 1;
        public                  int              acquisitionLevelOutOfClass = 1;
        public                  List<HeroClass>  difficultyBasicClasses;
        public                  List<HeroClass>  difficultyDemandingClasses;
        public                  Sprite           sprite;
        public                  int              level = 1;
        public                  SkillSubCategory subCategory;
        public                  SkillCategory    category;
        public                  SkillType        type;
        public                  int              cooldown;
        public                  int              manacostFlat;
        public                  float            manacostLevelScaling;
        public                  int              xpBaseBasic      = 16;
        public                  int              xpBaseDemanding  = 45;
        public                  int              xpBaseOutOfClass = 62;
        public                  string           displayName;
        public                  bool             appliesStun;
        [TextArea(4, 4)] public string           description;
        [Header("Effect Scaling, multiplicative")]
        public float dStrength;
        public                     float            dConstitution;
        public                     float            dDexterity;
        public                     float            dQuickness;
        public                     float            dIntuition;
        public                     float            dLogic;
        public                     float            dWillpower;
        public                     float            dWisdom;
        public                     float            dCharisma;
        public                     float            dLevel      = 0.5f;
        public                     float            dMultiplier = 1;
        public                     int              addedFlatDamage;
        public                     GameObject       weapon;
        [Header("0 bis 1")] public float            damageRange;
        public                     int              Manacost     => (int)(manacostFlat + level * manacostLevelScaling);
        public                     bool             CanParryWith => weapon is not null;

        protected string Description
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

        public int GetAcquisitionLevel(Hero hero)
        {
            var difficulty = GetDifficultyByHero(hero);

            return difficulty switch
            {
                SkillDifficulty.Basic => acquisitionLevelBasic,
                SkillDifficulty.Demanding => acquisitionLevelDemanding,
                SkillDifficulty.OutOfClass => acquisitionLevelOutOfClass,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public SkillDifficulty GetDifficultyByHero(Hero hero)
        {
            var heroclass = hero.heroClass;

            if (difficultyBasicClasses.Any(db => db.name == heroclass.name))
                return SkillDifficulty.Basic;

            return difficultyDemandingClasses.Any(dd => dd.name == heroclass.name) ? SkillDifficulty.Demanding : SkillDifficulty.OutOfClass;
        }

        public abstract string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult);

        public virtual string GetTooltip(BaseHero hero, string damage = "0-0") => $"<b>{displayName.ToUpper()}</b>{Environment.NewLine}" +
                                                                                  $"<i>{category}, {subCategory}</i>{Environment.NewLine}{Environment.NewLine}" +
                                                                                  GetManacostText(hero) +
                                                                                  GetDamageText(damage);

        private string GetManacostText(BaseHero hero)
        {
            if (Manacost == 0)
                return string.Empty;

            var hexColor = Manacost > hero.CurrentMana ? Konstanten.NotEnoughManaColor : Konstanten.EnoughManaColor;

            return $"Manacost:\t<b><color={hexColor}>{Manacost}</color></b>{Environment.NewLine}";
        }

        private string GetDamageText(string damage) => damage == "0-0" ? string.Empty : $"Damage:\t<b>{damage}</b>{Environment.NewLine}";
    }
}