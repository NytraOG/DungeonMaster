using System;
using System.Linq;
using Abilities;
using Entities.Enums;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemies
{
    public abstract class BaseFoe : BaseUnit
    {
        public override Party Party => Party.Foe;

        public int strength;
        public int constitution;
        public int dexterity;
        public int quickness;
        public int intuition;
        public int logic;
        public int willpower;
        public int wisdom;
        public int charisma;

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

            abilities.Add(ScriptableObject.CreateInstance<Shiv>());
        }

        private void FixedUpdate()
        {
            if(abilities.All(a => a.AbilityName != AbilityNames.Shiv))
                abilities.Add(ScriptableObject.CreateInstance<Shiv>());
        }

        public void PickAbility()
        {
            if (!abilities.Any())
                return;

            var rnadomNoKek = new Random().Next(0, abilities.Count);

            SelectedAbility = abilities[rnadomNoKek];
        }
    }
}