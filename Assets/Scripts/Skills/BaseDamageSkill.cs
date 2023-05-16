using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Buffs;
using UnityEngine;
using UnityEngine.Events;

namespace Skills
{
    public abstract class BaseDamageSkill : BaseTargetingSkill
    {
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
                                                        actor.Charisma * hCharisma) *
                                                       hMultiplier *
                                                       GetModifier(this, actor)).InfuseRandomness();

        public (int, int) GetDamage(BaseUnit actor)
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
                               addedFlatDamage +
                               actor.FlatDamageModifier);

            var minhit = (int)(maxhit * (1 - damageRange));

            return (minhit, maxhit);
        }

        protected void ApplyDebuffs(BaseUnit actor, BaseUnit target)
        {
            if (!appliesDebuff)
                return;

            foreach (var debuff in appliedDebuffs)
            {
                if (target.debuffs.Any(b => b.displayname == debuff.displayname))
                    target.debuffs.First(b => b.displayname == debuff.displayname).currentDuration += debuff.duration;
                else
                {
                    debuff.appliedBy   = this;
                    debuff.appliedFrom = actor;
                    target.debuffs.Add(debuff);
                    debuff.ApplyDamageModifier(target);
                    debuff.ApplyRatingModifier(target);
                }
            }
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