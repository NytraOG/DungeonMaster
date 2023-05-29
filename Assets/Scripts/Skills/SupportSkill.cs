using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Buffs;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Supportskill", menuName = "Skills/Support")]
    public class SupportSkill : BaseTargetingSkill
    {
        public                                       List<Buff>          appliedBuffs   = new();
        public                                       List<Debuff>        appliedDebuffs = new();
        public                                       bool                selfcastOnly;
        public                                       int                 actionsModifier;
        public                                       float               flatDamageModifier;
        [Header("Attackratings, additive")] public   float               meleeAttackratingModifier;
        public                                       float               rangedAttackratingModifier;
        public                                       float               magicAttackratingModifier;
        public                                       float               socialAttackratingModifier;
        [Header("Defensemodifier, additive")] public float               meleeDefensmodifier;
        public                                       float               rangedDefensemodifier;
        public                                       float               magicDefensemodifier;
        public                                       float               socialDefensemodifier;
        public                                       List<SkillCategory> affectedCategories = new();
        public override                              Factions            TargetableFaction => Factions.All;

        protected virtual void Awake()
        {
            category    = SkillCategory.Support;
            subCategory = SkillSubCategory.Special;
        }

        public override string Activate(BaseUnit _, BaseUnit target)
        {
            foreach (var skillCategory in affectedCategories)
            {
                switch (skillCategory)
                {
                    case SkillCategory.Melee:
                        target.MeleeAttackratingModifier += meleeAttackratingModifier;
                        target.FlatDamageModifier        += flatDamageModifier;
                        target.MeleeDefensmodifier       += meleeDefensmodifier;
                        target.MeleeDefense              += GetTacticalRoll(target);

                        break;
                    case SkillCategory.Ranged:
                        target.RangedAttackratingModifier += rangedAttackratingModifier;
                        target.FlatDamageModifier         += flatDamageModifier;
                        target.RangedDefensemodifier      += rangedDefensemodifier;
                        target.RangedDefense              += GetTacticalRoll(target);
                        break;
                    case SkillCategory.Magic:
                        target.MagicAttackratingModifier += magicAttackratingModifier;
                        target.FlatDamageModifier        += flatDamageModifier;
                        target.MagicDefensemodifier      += magicDefensemodifier;
                        target.MagicDefense              += GetTacticalRoll(target);
                        break;
                    case SkillCategory.Social:
                        target.SocialAttackratingModifier += socialAttackratingModifier;
                        target.SocialDefensemodifier      += socialDefensemodifier;
                        target.SocialDefense              += GetTacticalRoll(target);
                        break;
                    case SkillCategory.Summon:     break;
                    case SkillCategory.Initiative: break;
                    default:                       throw new ArgumentOutOfRangeException();
                }
            }

            PopulateBuffs(target);

            if (!target.activeSkills.ContainsKey(this))
                target.activeSkills.Add(this, false);

            return $"Activated {name}";
        }

        public void Reverse(BaseUnit target)
        {
            foreach (var skillCategory in affectedCategories)
            {
                switch (skillCategory)
                {
                    case SkillCategory.Melee:
                        target.MeleeAttackratingModifier -= meleeAttackratingModifier;
                        target.FlatDamageModifier        -= flatDamageModifier;
                        target.MeleeDefensmodifier       -= meleeDefensmodifier;
                        target.MeleeDefense              -= GetTacticalRoll(target);
                        break;
                    case SkillCategory.Ranged:
                        target.RangedAttackratingModifier -= rangedAttackratingModifier;
                        target.FlatDamageModifier         -= flatDamageModifier;
                        target.RangedDefensemodifier      -= rangedDefensemodifier;
                        target.RangedDefense              -= GetTacticalRoll(target);
                        break;
                    case SkillCategory.Magic:
                        target.MagicAttackratingModifier -= magicAttackratingModifier;
                        target.FlatDamageModifier        -= flatDamageModifier;
                        target.MagicDefensemodifier      -= magicDefensemodifier;
                        target.MagicDefense              -= GetTacticalRoll(target);
                        break;
                    case SkillCategory.Social:
                        target.SocialAttackratingModifier -= socialAttackratingModifier;
                        target.SocialDefensemodifier      -= socialDefensemodifier;
                        target.SocialDefense              -= GetTacticalRoll(target);
                        break;
                    case SkillCategory.Summon:     break;
                    case SkillCategory.Initiative: break;
                    default:                       throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void PopulateBuffs(BaseUnit actor)
        {
            if (appliedBuffs.Any())
                ApplyBuffs(actor, actor);
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

        public override string Activate(BaseUnit actor) => throw new NotImplementedException();
    }
}