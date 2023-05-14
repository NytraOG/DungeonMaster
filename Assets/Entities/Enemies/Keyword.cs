using System.Collections.Generic;
using System.Linq;
using Skills;
using UnityEngine;

namespace Entities.Enemies
{
    [CreateAssetMenu(fileName = "Keyword")]
    public class Keyword : ScriptableObject
    {
        public string          displayname;
        public float           strengthModifier;
        public float           constitutionModifier;
        public float           dexterityModifier;
        public float           quicknessModifier;
        public float           intuitionModifier;
        public float           logicModifier;
        public float           wisdomModifier;
        public float           willpowerModifier;
        public float           charismaModifier;
        public List<BaseSkill> skills;

        public void ApplyValues(Creature creature)
        {
            creature.Strength     = (int)(creature.Strength * strengthModifier);
            creature.Constitution = (int)(creature.Constitution * constitutionModifier);
            creature.Dexterity    = (int)(creature.Dexterity * dexterityModifier);
            creature.Quickness    = (int)(creature.Quickness * quicknessModifier);
            creature.Intuition    = (int)(creature.Intuition * intuitionModifier);
            creature.Logic        = (int)(creature.Logic * logicModifier);
            creature.Wisdom       = (int)(creature.Wisdom * wisdomModifier);
            creature.Willpower    = (int)(creature.Willpower * willpowerModifier);
            creature.Charisma     = (int)(creature.Charisma * charismaModifier);
        }

        public void PopulateSkills(Creature creature)
        {
            foreach (var skill in skills)
            {
                if (creature.skills.All(s => s.displayName != skill.displayName))
                    creature.skills.Add(skill);
            }
        }
    }
}