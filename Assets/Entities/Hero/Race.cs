using System.Collections.Generic;
using System.Linq;
using Skills;
using UnityEngine;

namespace Entities.Hero
{

    [CreateAssetMenu(fileName = "HeroRace", menuName = "Hero/Race")]
    public class Race : ScriptableObject
    {
        public Sprite          sprite;
        public string          displayName;
        public int             modifierStrength;
        public int             modifierConstitution;
        public int             modifierDexterity;
        public int             modifierQuickness;
        public int             modifierIntuition;
        public int             modifierLogic;
        public int             modifierWillpower;
        public int             modifierWisdom;
        public int             modifierCharisma;
        public List<BaseSkill> skills;

        public void ApplyModifiers(BaseUnit unit)
        {
            unit.Strength     += modifierStrength;
            unit.Constitution += modifierConstitution;
            unit.Dexterity    += modifierDexterity;
            unit.Quickness    += modifierQuickness;
            unit.Intuition    += modifierIntuition;
            unit.Logic        += modifierLogic;
            unit.Willpower    += modifierWillpower;
            unit.Wisdom       += modifierWisdom;
            unit.Charisma     += modifierCharisma;
        }

        public void ApplySkills(BaseUnit unit) => skills.ForEach(s =>
        {
            if (unit.skills.Any(a => a.name == s.name))
                return;

            if(s is SupportSkill supportSkill)
                supportSkill.PopulateBuffs(unit);

            unit.skills.Add(s);
        });
    }
}