using System;
using Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Skills
{
    public abstract class BaseDamageSkill : BaseSkill
    {
        [Header("Hitroll Multiplier")] public int                 hStrength;
        public                                int                 hConstitution;
        public                                int                 hDexterity;
        public                                int                 hQuickness;
        public                                int                 hIntuition;
        public                                int                 hLogic;
        public                                int                 hWillpower;
        public                                int                 hWisdom;
        public                                int                 hCharisma;
        public                                float               hMultiplier = 1;
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

        private float GetModifier(BaseDamageSkill baseDamageSkill, BaseUnit actor) => baseDamageSkill switch
        {
            MeleeSkill => actor.MeleeAttackratingModifier,
            RangedSkill => actor.RangedAttackratingModifier,
            MagicSkill => actor.MagicAttackratingModifier,
            _ => throw new ArgumentOutOfRangeException(nameof(baseDamageSkill))
        };
    }
}