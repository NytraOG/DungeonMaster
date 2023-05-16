using System;
using System.Collections.Generic;
using System.Linq;
using Battlefield;
using Entities.Enemies.Monstertypes;
using Entities.Enums;
using Skills;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemies
{
    public class Creature : BaseUnit
    {
        public string          displayname;
        public float           levelModifier;
        public BaseMonstertype monstertype;
        public List<Keyword>   keywords;
        public List<Positions> favouritePositions = new () { Positions.None };

        public override Party           Party => Party.Foe;

        protected override void Awake()
        {
            displayname = $"{monstertype.displayname} {keywords[0]?.displayname}";
            name        = displayname;

            monstertype.ApplyValues(this);

            SetAttributeByLevel();

            base.Awake();

            ApplyKeywords();
            SetInitialHitpointsAndMana();

            var spriterenderer = GetComponent<SpriteRenderer>();
            spriterenderer.sprite ??= monstertype.sprite;

            unitTooltip = GameObject.Find("UiCanvas").transform.Find("UnitTooltip").gameObject;
        }

        private void OnMouseDown()
        {
            var controller = FindObjectOfType<BattleController>();
            controller.selectedTarget = this;

            Debug.Log($"{name} clicked");
        }

        public override (int, int) GetApproximateDamage(BaseSkill ability) => ability switch
        {
            BaseDamageSkill skill => skill.GetDamage(this),
            SupportSkill _ => (0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(ability))
        };

        public override string UseAbility(BaseSkill ability, BaseUnit target = null)
        {
            var dmg = ability.Activate(this, target);
            SelectedSkill = null;

            return dmg;
        }

        private void ApplyKeywords()
        {
            foreach (var keyword in keywords)
            {
                keyword.ApplyAttributeModifier(this);
                keyword.ApplyRatingModifier(this);
                keyword.ApplyDamageModifier(this);
                keyword.PopulateSkills(this);
            }
        }

        public void PickSkill()
        {
            if (!skills.Any())
                return;

            var abilityIndex = new Random().Next(0, skills.Count);

            SelectedSkill = skills[abilityIndex];
        }

        private void SetAttributeByLevel()
        {
            if(level == 1)
                return;

            var modifier = levelModifier * level;

            Strength     += (int)(monstertype.strength * modifier);
            Constitution += (int)(monstertype.constitution * modifier);
            Dexterity    += (int)(monstertype.dexterity * modifier);
            Quickness    += (int)(monstertype.quickness * modifier);
            Intuition    += (int)(monstertype.intuition * modifier);
            Logic        += (int)(monstertype.logic * modifier);
            Wisdom       += (int)(monstertype.wisdom * modifier);
            Willpower    += (int)(monstertype.willpower * modifier);
            Charisma     += (int)(monstertype.charisma * modifier);
        }
    }
}