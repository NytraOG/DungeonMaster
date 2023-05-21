using System.Collections.Generic;
using Entities.Buffs;
using Entities.Enums;
using Skills;
using UnityEngine;

namespace Entities
{
    public abstract class BaseUnit : Base
    {
        public          GameObject                     healthbarPrefab;
        public          int                            xpAvailable;
        public          int                            xpSpent;
        public          int                            xpBaseAttribut = 50;
        public          int                            level          = 1;
        public          int                            strength;
        public          int                            constitution;
        public          int                            dexterity;
        public          int                            quickness;
        public          int                            intuition;
        public          int                            logic;
        public          int                            willpower;
        public          int                            wisdom;
        public          int                            charisma;
        public          List<BaseSkill>                skills  = new();
        public          List<Buff>                     buffs   = new();
        public          List<Debuff>                   debuffs = new();
        public          HealthpointBar                 healthbarInstance;
        public          GameObject                     unitTooltip;
        public          Dictionary<SupportSkill, bool> activeSkills = new();
        public abstract Party                          Party                      { get; }
        public          BaseSkill                      SelectedSkill              { get; set; }
        public          int                            AktionenGesamt             { get; set; }
        public          int                            AktionenAktuell            { get; set; }
        public          float                          ManaregenerationRate       { get; set; }
        public          int                            Armour                     { get; set; }
        public          float                          MeleeAttackratingModifier  { get; set; }
        public          float                          RangedAttackratingModifier { get; set; }
        public          float                          MagicAttackratingModifier  { get; set; }
        public          float                          SocialAttackratingModifier { get; set; }
        public          int                            ActionsModifier            { get; set; }
        public          float                          FlatDamageModifier         { get; set; }
        public          float                          MeleeDefensmodifier        { get; set; }
        public          float                          ModifiedMeleeDefense       => MeleeDefense * MeleeDefensmodifier;
        public          float                          RangedDefensemodifier      { get; set; }
        public          float                          ModifiedRangedDefense      => RangedDefense * RangedDefensemodifier;
        public          float                          MagicDefensemodifier       { get; set; }
        public          float                          ModifiedMagicDefense       => MagicDefense * MagicDefensemodifier;
        public          float                          SocialDefensemodifier      { get; set; }
        public          float                          ModifiedSocialDefense      => SocialDefense * SocialDefensemodifier;
        public          float                          MagicDefense               { get; set; }
        public          float                          SocialDefense              { get; set; }
        public          float                          MeleeDefense               { get; set; }
        public          float                          RangedDefense              { get; set; }
        public          float                          CurrentMana                { get; set; }
        public          float                          MaximumMana                { get; set; }
        public          float                          CurrentHitpoints           { get; set; }
        public          float                          MaximumHitpoints           { get; set; }
        public          float                          CurrentInitiative          { get; set; }
        public          bool                           IsStunned                  { get; set; }
        public          bool                           IsDead                     => CurrentHitpoints <= 0;
        public          bool                           IstKampfunfähig            => CurrentHitpoints <= 0;
        public          float                          BaseMeleeDefense           => 2 * Dexterity + Quickness;
        public          float                          BaseRangedDefense          => 2 * Quickness + Dexterity;
        public          float                          BaseMagicDefense           => 2 * Willpower + Wisdom;
        public          float                          BaseSocialDefense          => 2 * Logic + Charisma;
        public          float                          BaseInitiative             => 2 * Intuition + Quickness;
        public          int                            XpToSpendForLevelUp        => this.GetXpToSpendForLevelUp();
        public          string                         CombatLogColor             => this is Hero.Hero ? Konstanten.HeroDamageColor : Konstanten.CreatureColor;

        protected virtual void Awake()
        {
            MagicDefense  = BaseMagicDefense;
            SocialDefense = BaseSocialDefense;
            MeleeDefense  = BaseMeleeDefense;
            RangedDefense = BaseRangedDefense;

            AktionenGesamt  = 1;
            AktionenAktuell = AktionenGesamt;
        }

        private void OnMouseEnter() => ShowUnitTooltip();

        private void OnMouseExit() => HideHealthbar();

        private void ShowUnitTooltip()
        {
            unitTooltip.SetActive(true);
            unitTooltip.GetComponent<UnitTooltip>().unit = this;
        }

        protected void SetInitialHitpointsAndMana()
        {
            MaximumMana      = 2 * Wisdom + Logic;
            CurrentMana      = MaximumMana;
            MaximumHitpoints = 3 * Constitution + 2 * Strength;
            CurrentHitpoints = MaximumHitpoints;
        }

        public void HideHealthbar()
        {
            unitTooltip.SetActive(false);
            unitTooltip.GetComponent<UnitTooltip>().unit = null;

            Debug.Log($"Pointer entered {name}");
        }

        public abstract (int, int) GetApproximateDamage(BaseSkill ability);

        public abstract string UseAbility(BaseSkill ability, HitResult hitResult, BaseUnit target = null);

        public virtual void InitiativeBestimmen(double modifier = 1)
        {
            var initiative = (float)modifier * BaseInitiative.InfuseRandomness();

            CurrentInitiative = initiative;
        }

        public virtual void Initialize() => CurrentHitpoints = MaximumHitpoints;

        #region Stats

        public int Strength     { get; set; }
        public int Constitution { get; set; }
        public int Dexterity    { get; set; }
        public int Wisdom       { get; set; }
        public int Quickness    { get; set; }
        public int Intuition    { get; set; }
        public int Logic        { get; set; }
        public int Willpower    { get; set; }
        public int Charisma     { get; set; }

        #endregion
    }
}