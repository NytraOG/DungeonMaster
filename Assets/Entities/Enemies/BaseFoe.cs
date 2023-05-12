using System;
using System.Linq;
using Entities.Enums;
using Skills;

namespace Entities.Enemies
{
    public abstract class BaseFoe : BaseUnit
    {
        public          MeleeSkill meleeSkill;
        public          string[]   firstnames;
        public          string[]   lastnames;
        public override Party      Party => Party.Foe;

        protected override void Awake()
        {
            GenerateName();

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

            base.Awake();
        }

        private void FixedUpdate()
        {
            if (skills.All(a => a.displayName != meleeSkill.displayName))
                skills.Add(meleeSkill);
        }

        public void PickSkill()
        {
            if (!skills.Any())
                return;

            var abilityIndex = new Random().Next(0, skills.Count);

            SelectedSkill = skills[abilityIndex];
        }

        private void GenerateName()
        {
            var rng = new Random();

            var firstname = firstnames[rng.Next(0, firstnames.Length)];
            var lastname  = lastnames[rng.Next(0, lastnames.Length)];

            var istDoppelnameKek = rng.Next(0, 5) > 4;

            if (istDoppelnameKek)
                firstname += $"-{firstnames[rng.Next(0, firstnames.Length)]}";

            name = $"{firstname} {lastname}";
        }
    }
}