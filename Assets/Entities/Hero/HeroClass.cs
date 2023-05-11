using System.Collections.Generic;
using System.Linq;
using Skills;
using UnityEngine;

namespace Entities.Hero
{
    [CreateAssetMenu(fileName = "HeroClass", menuName = "Hero/Class")]
    public class HeroClass : ScriptableObject
    {
        [Header("Modifier in Prozent: [-1, .., 1]")]
        public float meleeAttack;
        public float           rangedAttack;
        public float           magicAttack;
        public float           socialAttack;
        public float           meleeDefense;
        public float           rangedDefense;
        public float           magicDefense;
        public float           socialDefense;
        public float           health;
        public float           mana;
        public float           explosion;
        public List<BaseSkill> skills;

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
            unit.CurrentHitpoints           =  unit.MaximumHitpoints;
            unit.MaximumMana                += unit.MaximumMana * mana;
            unit.CurrentMana                =  unit.MaximumMana;
        }

        public void ApplySkills(BaseUnit unit) => skills.ForEach(s =>
        {
            if (unit.skills.All(a => a.name != s.name))
                unit.skills.Add(s);
        });
    }
}