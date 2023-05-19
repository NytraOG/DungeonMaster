using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Buffs;
using Entities.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Skills
{
    public abstract class BaseDamageSkill : BaseTargetingSkill
    {
        public                                int                 bonusDamageNormal;
        public                                int                 bonusDamageGood;
        public                                int                 bonusDamageCritical;
        public                                bool                appliesDebuff;
        [Header("Hitroll Multiplier")] public int                 hStrength;
        public                                int                 hConstitution;
        public                                int                 hDexterity;
        public                                int                 hQuickness;
        public                                int                 hIntuition;
        public                                int                 hLogic;
        public                                int                 hWillpower;
        public                                int                 hWisdom;
        public                                int                 hCharisma;
        public                                int                 hLevel = 2;
        public                                float               hMultiplier    = 1;
        public                                List<Debuff>        appliedDebuffs = new();
        public                                UnityAction<string> OnDamageDealt;

        public int GetHitroll(BaseUnit actor) => (int)((actor.Strength * hStrength +
                                                        actor.Constitution * hConstitution +
                                                        actor.Dexterity * hDexterity +
                                                        actor.Quickness * hQuickness +
                                                        actor.Intuition * hIntuition +
                                                        actor.Logic * hLogic +
                                                        actor.Willpower * hWillpower +
                                                        actor.Wisdom * hWisdom +
                                                        actor.Charisma * hCharisma +
                                                        level * hLevel) *
                                                       hMultiplier *
                                                       GetModifier(this, actor)).InfuseRandomness();

        public (int, int) GetDamage(BaseUnit actor, HitResult hitresult)
        {
            var maxhit = (int)(actor.Strength * dStrength +
                               actor.Constitution * dConstitution +
                               actor.Dexterity * dDexterity +
                               actor.Quickness * dQuickness +
                               actor.Intuition * dIntuition +
                               actor.Logic * dLogic +
                               actor.Willpower * dWillpower +
                               actor.Wisdom * dWisdom +
                               actor.Charisma * dCharisma +
                               level * dLevel +
                               addedFlatDamage +
                               actor.FlatDamageModifier);

            maxhit += hitresult switch
            {
                HitResult.None => 0,
                HitResult.Normal => bonusDamageNormal,
                HitResult.Good => bonusDamageGood,
                HitResult.Critical => bonusDamageCritical,
                _ => throw new ArgumentOutOfRangeException(nameof(hitresult), hitresult, null)
            };

            var minhit = (int)(maxhit * (1 - damageRange));

            return (minhit, maxhit);
        }

        protected void ApplyDebuffs(BaseUnit actor, BaseUnit target)
        {
            if (!appliesDebuff)
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

        private float GetModifier(BaseDamageSkill baseDamageSkill, BaseUnit actor) => baseDamageSkill switch
        {
            MeleeSkill => actor.MeleeAttackratingModifier,
            RangedSkill => actor.RangedAttackratingModifier,
            MagicSkill => actor.MagicAttackratingModifier,
            _ => throw new ArgumentOutOfRangeException(nameof(baseDamageSkill))
        };
    }
}