using System.Collections.Generic;
using System.Linq;
using Skills;
using UnityEngine;

namespace Entities.Enemies
{
    [CreateAssetMenu(fileName = "Keyword")]
    public class Keyword : ScriptableObject
    {
        [Header("Attributes")] public string          displayname;
        public                        float           strengthMultiplier     = 1;
        public                        float           constitutionMultiplier = 1;
        public                        float           dexterityMultiplier    = 1;
        public                        float           quicknessMultiplier    = 1;
        public                        float           intuitionMultiplier    = 1;
        public                        float           logicMultiplier        = 1;
        public                        float           wisdomMultiplier       = 1;
        public                        float           willpowerMultiplier    = 1;
        public                        float           charismaMultiplier     = 1;
        [Header("Ratings")] public    int             actionsModifier;
        public                        float           flatDamageModifier;
        public                        float           meleeAttackratingModifier;
        public                        float           rangedAttackratingModifier;
        public                        float           magicAttackratingModifier;
        public                        float           socialAttackratingModifier;
        public                        float           meleeDefensmodifier;
        public                        float           rangedDefensemodifier;
        public                        float           magicDefensemodifier;
        public                        float           socialDefensemodifier;
        public                        List<BaseSkill> skills;

        public void ApplyAttributeModifier(Creature creature)
        {
            creature.Strength     *= (int)strengthMultiplier;
            creature.Constitution *= (int)constitutionMultiplier;
            creature.Dexterity    *= (int)dexterityMultiplier;
            creature.Quickness    *= (int)quicknessMultiplier;
            creature.Intuition    *= (int)intuitionMultiplier;
            creature.Logic        *= (int)logicMultiplier;
            creature.Wisdom       *= (int)wisdomMultiplier;
            creature.Willpower    *= (int)willpowerMultiplier;
            creature.Charisma     *= (int)charismaMultiplier;
        }

        public void ApplyRatingModifier(Creature creature)
        {
            creature.MeleeAttackratingModifier  += meleeAttackratingModifier;
            creature.RangedAttackratingModifier += rangedAttackratingModifier;
            creature.MagicAttackratingModifier  += magicAttackratingModifier;
            creature.SocialAttackratingModifier += socialAttackratingModifier;

            creature.MeleeDefensmodifier   += meleeDefensmodifier;
            creature.RangedDefense         += rangedDefensemodifier;
            creature.MagicDefensemodifier  += magicDefensemodifier;
            creature.SocialDefensemodifier += socialDefensemodifier;
        }

        public void ApplyDamageModifier(Creature creature)
        {
            creature.AktionenGesamt     += actionsModifier;
            creature.AktionenAktuell    =  creature.AktionenGesamt;
            creature.FlatDamageModifier +=  flatDamageModifier;
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