using System.Linq;
using Entities.Enums;
using Skills;
using Skills.neu;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemies
{
    public abstract class BaseFoe : BaseUnit
    {
        public          MeleeSkill meleeSkill;
        public override Party      Party => Party.Foe;

        public void Awake()
        {
            Strength     = strength;
            Constitution = constitution;
            Dexterity    = dexterity;
            Quickness    = quickness;
            Intuition    = intuition;
            Logic        = logic;
            Willpower    = willpower;
            Wisdom       = wisdom;
            Charisma     = charisma;
            
            SetInitialHitpointsAndMana();

            skills.Add(meleeSkill);
        }

        private void FixedUpdate()
        {
            if (skills.All(a => a.displayName != meleeSkill.displayName))
                skills.Add(meleeSkill);
        }

        public void PickAbility()
        {
            if (!skills.Any())
                return;

            var abilityIndex = new Random().Next(0, skills.Count);

            SelectedSkill = skills[abilityIndex];
        }
    }
}