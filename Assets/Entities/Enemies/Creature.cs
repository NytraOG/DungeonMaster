using System;
using System.Collections.Generic;
using System.Linq;
using Battlefield;
using Entities.Enemies.Monstertypes;
using Entities.Enums;
using Skills;
using UnityEngine;

namespace Entities.Enemies
{
    public class Creature : BaseUnit
    {
        public          string          displayname;
        public          float           levelModifier;
        public          BaseMonstertype monstertype;
        public          List<Keyword>   keywords;
        public          List<Positions> favouritePositions = new() { Positions.None };
        public override Party           Party => Party.Foe;

        protected override void Awake()
        {
            displayname = $"{monstertype.displayname} {keywords[0]?.displayname}";
            name        = displayname;

            MeleeAttackratingModifier  = 1;
            RangedAttackratingModifier = 1;
            MagicAttackratingModifier  = 1;
            SocialAttackratingModifier = 1;

            MeleeDefensmodifier   = 1;
            RangedDefensemodifier = 1;
            MagicDefensemodifier  = 1;
            SocialDefensemodifier = 1;

            InitiativeModifier = 1;

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
            Debug.Log($"{name} clicked");

            var controller = FindObjectOfType<BattleController>();

            switch (controller.selectedSkill)
            {
                case null: return;
                case BaseTargetingSkill targetingSkill:
                {
                    var maxTargets = targetingSkill.GetTargets(controller.selectedHero);

                    var maxTargetsReached = controller.selectedTargets.Count == maxTargets;

                    if (maxTargetsReached)
                    {
                        controller.ShowToast($"Target maximum {maxTargets} reached for {targetingSkill.displayName}");
                        return;
                    }

                    break;
                }
            }

            GetComponent<SpriteRenderer>().material = controller.creatureOutlineMaterial;

            controller.selectedTargets.Add(this);
        }

        public override (int, int) GetApproximateDamage(BaseSkill ability) => ability switch
        {
            BaseDamageSkill skill => skill.GetDamage(this, HitResult.None),
            _ => throw new ArgumentOutOfRangeException(nameof(ability))
        };

        public override string UseAbility(BaseSkill skill, HitResult hitResult, BaseUnit target = null)
        {
            var dmg = skill.Activate(this, target, hitResult);

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

            foreach (var skill in skills.OrderByDescending(s => s.Manacost))
            {
                if (skill.Manacost > CurrentMana)
                    continue;

                SelectedSkill = skill;
                return;
            }
        }

        private void SetAttributeByLevel()
        {
            if (level == 1)
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