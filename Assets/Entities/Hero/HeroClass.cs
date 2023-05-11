using System.Collections.Generic;
using System.Linq;
using Skills;
using UnityEngine;

namespace Entities.Hero
{
    [CreateAssetMenu(fileName = "HeroClass", menuName = "Hero/Class")]
    public class HeroClass : ScriptableObject
    {
        [Header("Modifier in Prozent")] [Range(0, 1)]
        public float meleeAttack;
        [Range(0, 1)] public float           rangedAttack;
        [Range(0, 1)] public float           magicAttack;
        [Range(0, 1)] public float           socialAttack;
        [Range(0, 1)] public float           meleeDefense;
        [Range(0, 1)] public float           rangedDefense;
        [Range(0, 1)] public float           magicDefense;
        [Range(0, 1)] public float           socialDefense;
        [Range(0, 1)] public float           health;
        [Range(0, 1)] public float           mana;
        [Range(0, 1)] public float           explosion;
        public               List<BaseSkill> skills;

        public void ApplyModifiers(BaseUnit unit)
        {
            unit.MeleeAttackratingModifier  =  meleeAttack;
            unit.RangedAttackratingModifier =  rangedAttack;
            unit.MagicAttackratingModifier  =  magicAttack;
            unit.SocialAttackratingModifier =  socialAttack;
            unit.MeleeDefense               += unit.MeleeDefense * meleeDefense;
            unit.RangedDefense              += unit.RangedDefense * rangedDefense;
            unit.MagicDefense               += unit.MagicDefense * magicDefense;
            unit.SocialDefense              += unit.SocialDefense * socialDefense;
            unit.MaximumHitpoints           += unit.MaximumHitpoints * health;
            unit.MaximumMana                += unit.MaximumMana * mana;
        }

        public void ApplySkills(BaseUnit unit) => skills.ForEach(s =>
        {
            if (unit.skills.All(a => a.name != s.name))
                unit.skills.Add(s);
        });
    }
}