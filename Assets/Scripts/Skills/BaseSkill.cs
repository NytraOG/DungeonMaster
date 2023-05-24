using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Enums;
using Entities.Hero;
using UnityEngine;
using Attribute = Entities.Enums.Attribute;

namespace Skills
{
    public abstract class BaseSkill : ScriptableObject
    {
        public                           SkillCategory    category;
        public                           SkillSubCategory subCategory;
        public                           SkillType        type;
        [Header("")] public              string           displayName;
        public                           Sprite           sprite;
        public                           GameObject       weapon;
        [TextArea(4, 4)]     public      string           description;
        [Header("Leveling")] public      int              level                          = 1;
        public                           int              acquisitionLevelHeroBasic      = 1;
        public                           int              acquisitionLevelHeroDemanding  = 1;
        public                           int              acquisitionLevelOutOfHeroClass = 1;
        public                           List<HeroClass>  difficultyBasicClasses;
        public                           List<HeroClass>  difficultyDemandingClasses;
        public                           int              xpBaseBasic      = 16;
        public                           int              xpBaseDemanding  = 45;
        public                           int              xpBaseOutOfClass = 62;
        public                           int              manacostFlat;
        public                           float            manacostLevelScaling;
        [Header("Tactical Roll")] public Attribute        primaryAttributeT;
        public                           float            primaryScalingT = 2f;
        public                           Attribute        secondaryAttributeT;
        public                           float            secondaryScalingT  = 1f;
        public                           float            skillLevelScalingT = 2f;
        public                           float            multiplierT        = 1;

        public int Manacost => (int)(manacostFlat + level * manacostLevelScaling);
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

        public int GetTacticalRoll(BaseUnit unit)
        {
            var primaryValue   = primaryScalingT * unit.Get(primaryAttributeT);
            var secondaryValue = secondaryScalingT * unit.Get(secondaryAttributeT);
            var levelValue     = level * skillLevelScalingT;

            var tacticalRoll = (primaryValue + secondaryValue + levelValue).InfuseRandomness();
            var finalHitroll = tacticalRoll * multiplierT * GetAttackmodifier(this, unit);

            return (int)finalHitroll;
        }

        public int GetAcquisitionLevel(Hero hero)
        {
            var difficulty = GetDifficultyByHero(hero);

            return difficulty switch
            {
                SkillDifficulty.Basic => acquisitionLevelHeroBasic,
                SkillDifficulty.Demanding => acquisitionLevelHeroDemanding,
                SkillDifficulty.OutOfClass => acquisitionLevelOutOfHeroClass,
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

        protected float GetAttackmodifier(BaseSkill skill, BaseUnit actor) => skill.category switch
        {
            SkillCategory.Melee => actor.MeleeAttackratingModifier,
            SkillCategory.Ranged => actor.RangedAttackratingModifier,
            SkillCategory.Magic => actor.MagicAttackratingModifier,
            SkillCategory.Social => actor.SocialAttackratingModifier,
            SkillCategory.Summon => 0,
            SkillCategory.Support => 0,
            SkillCategory.Initiative => 0,
            _ => throw new ArgumentOutOfRangeException()
        };

        public abstract string Activate(BaseUnit actor, BaseUnit target, HitResult hitResult);

        public virtual string GetTooltip(BaseHero hero, string damage = "0-0") => $"<b>{displayName.ToUpper()}</b>{Environment.NewLine}" +
                                                                                  $"<i>{category}, {subCategory}, {type}</i>{Environment.NewLine}{Environment.NewLine}";
    }
}