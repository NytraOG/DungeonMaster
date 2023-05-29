using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Buffs;
using Entities.Enums;
using Entities.Hero;
using UnityEngine;
using Attribute = Entities.Enums.Attribute;

namespace Skills
{
    public abstract class BaseDamageSkill : BaseTargetingSkill
    {
        public                                     List<Debuff> appliedDebuffs = new();
        [Header("Hit Roll")] public                Attribute    primaryAttributeH;
        public                                     float        primaryScalingH = 2f;
        public                                     Attribute    secondaryAttributeH;
        public                                     float        secondaryScalingH  = 1f;
        public                                     float        skillLevelScalingH = 2f;
        public                                     float        multiplierH        = 1;
        [Header("Effect Roll")] public             Attribute    primaryAttributeD;
        public                                     float        primaryScalingD = 0.5f;
        public                                     Attribute    secondaryAttributeD;
        public                                     float        secondaryScalingD  = 0.34f;
        public                                     float        skillLevelScalingD = 0.5f;
        public                                     float        multiplierD        = 1;
        [Header("Hit Result added Damage")] public int          normal;
        public                                     int          good;
        public                                     int          critical;
        [Range(0, 1)] public                       float        damageRange;
        public                                     int          addedFlatDamage;

        public int GetHitroll(BaseUnit actor)
        {
            var primaryValue   = primaryScalingH * actor.Get(primaryAttributeH);
            var secondaryValue = secondaryScalingH * actor.Get(secondaryAttributeH);
            var levelValue     = level * skillLevelScalingH;

            var hitrollBase  = (primaryValue + secondaryValue + levelValue).InfuseRandomness();
            var finalHitroll = hitrollBase * multiplierH * GetAttackmodifier(this, actor);

            return (int)finalHitroll;
        }

        public (int, int) GetDamage(BaseUnit actor, HitResult hitresult)
        {
            var primaryValue   = primaryScalingD * actor.Get(primaryAttributeD);
            var secondaryValue = secondaryScalingD * actor.Get(secondaryAttributeD);
            var levelValue     = level * skillLevelScalingD;

            var maxhit = (primaryValue + secondaryValue + levelValue + addedFlatDamage) * multiplierD;

            maxhit += hitresult switch
            {
                HitResult.None => 0,
                HitResult.Normal => normal,
                HitResult.Good => good,
                HitResult.Critical => critical,
                _ => throw new ArgumentOutOfRangeException(nameof(hitresult), hitresult, null)
            };

            var minhit = (int)(maxhit * (1 - damageRange));

            return (minhit, (int)maxhit);
        }

        protected void ApplyDebuffs(BaseUnit actor, BaseUnit target)
        {
            if (!appliedDebuffs.Any())
                return;

            foreach (var debuff in appliedDebuffs)
            {
                if (target.debuffs.Any(d => d.displayname == debuff.displayname) && !debuff.isStackable)
                    continue;

                AddDebuff(actor, target, debuff);
            }
        }

        private void AddDebuff(BaseUnit actor, BaseUnit target, Debuff debuff)
        {
            var newInstance = debuff.ToNewInstance();

            newInstance.appliedBy   = this;
            newInstance.appliedFrom = actor;
            target.debuffs.Add(newInstance);
            newInstance.ApplyDamageModifier(target);
            newInstance.ApplyRatingModifier(target);
        }

        private string GetDamageText(string damage) => damage == "0-0" ? string.Empty : $"Damage:\t<b>{damage}</b>{Environment.NewLine}";

        public override string GetTooltip(BaseHero selectedHero, string damage = "0-0") => base.GetTooltip(selectedHero, damage) +
                                                                                           GetDamageText(damage) +
                                                                                           Environment.NewLine + Environment.NewLine +
                                                                                           Description;
    }
}