using System.Collections.Generic;
using System.Linq;
using Entities.Enums;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities
{
    public abstract class BaseUnit : Base
    {
        public          GameObject      healthbarPrefab;
        public          int             level;
        public          int             strength;
        public          int             constitution;
        public          int             dexterity;
        public          int             quickness;
        public          int             intuition;
        public          int             logic;
        public          int             willpower;
        public          int             wisdom;
        public          int             charisma;
        public          List<BaseSkill> skills = new();
        public          HealthpointBar  healthbarInstance;
        public          string          Name                       { get; }
        public abstract Party           Party                      { get; }
        public          BaseSkill       SelectedSkill              { get; set; }
        public          int             AktionenGesamt             { get; set; }
        public          int             AktionenAktuell            { get; set; }
        public          float           ManaregenerationRate       { get; set; }
        public          int             Armour                     { get; set; }
        public          int             CritDamage                 { get; set; }
        public          float           MeleeAttackratingModifier  { get; set; }
        public          float           RangedAttackratingModifier { get; set; }
        public          float           MagicAttackratingModifier  { get; set; }
        public          float           SocialAttackratingModifier { get; set; }
        public          int             ActionsModifier            { get; set; }
        public          float           FlatDamageModifier         { get; set; }
        public          float           MeleeDefensmodifier        { get; set; }
        public          float           RangedDefensemodifier      { get; set; }
        public          float           MagicDefensemodifier       { get; set; }
        public          float           SocialDefensemodifier      { get; set; }
        public          float           MagicDefense               { get; set; }
        public          float           SocialDefense              { get; set; }
        public          float           MeleeDefense               { get; set; }
        public          float           RangedDefense              { get; set; }
        public          float           CurrentMana                { get; set; }
        public          float           MaximumMana                { get; set; }
        public          float           CurrentHitpoints           { get; set; }
        public          float           MaximumHitpoints           { get; set; }
        public          float           CurrentInitiative          { get; set; }
        public          bool            IsStunned                  { get; set; }
        public          bool            IsDead                     => CurrentHitpoints <= 0;
        public          bool            IstKampfunfähig            => CurrentHitpoints <= 0;
        public          float           BaseMeleeDefense           => 2 * Dexterity + Quickness;
        public          float           BaseRangedDefense          => 2 * Quickness + Dexterity;
        public          float           BaseMagicDefense           => 2 * Willpower + Wisdom;
        public          float           BaseSocialDefense          => 2 * Logic + Charisma;
        public          float           BaseInitiative             => 2 * Intuition + Quickness;

        protected virtual void Awake()
        {
            MagicDefense  = BaseMagicDefense;
            SocialDefense = BaseSocialDefense;
            MeleeDefense  = BaseMeleeDefense;
            RangedDefense = BaseRangedDefense;

            MeleeAttackratingModifier  = 1;
            RangedAttackratingModifier = 1;
            MagicAttackratingModifier  = 1;
            SocialAttackratingModifier = 1;

            MeleeDefensmodifier   = 1;
            RangedDefensemodifier = 1;
            MagicDefensemodifier  = 1;
            SocialDefensemodifier = 1;

            AktionenGesamt  = 1;
            AktionenAktuell = AktionenGesamt;
        }

        private void OnMouseEnter() => ShowHealthbar();

        private void OnMouseExit() => HideHealthbar();

        protected void SetInitialHitpointsAndMana()
        {
            MaximumMana      = 2 * Wisdom + Logic;
            CurrentMana      = MaximumMana;
            MaximumHitpoints = 3 * Constitution + 2 * Strength;
            CurrentHitpoints = MaximumHitpoints;
        }

        private void ShowCharPanel()
        {
            var panel = SceneManager.GetActiveScene()
                                    .GetRootGameObjects()
                                    .First(x => x.name == "Battlefield")
                                    .transform.Find("UI")
                                    .transform.Find("Canvas")
                                    .transform.Find("CharacterSheetPanel");

            panel.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text     = Name;
            panel.transform.Find("StrValue").gameObject.GetComponent<TextMeshProUGUI>().text = Strength.ToString();
            panel.transform.Find("DexValue").gameObject.GetComponent<TextMeshProUGUI>().text = Dexterity.ToString();
            panel.transform.Find("HPValue").gameObject.GetComponent<TextMeshProUGUI>().text  = $"{(int)CurrentHitpoints} / {(int)MaximumHitpoints}";

            panel.gameObject.SetActive(true);
        }

        public void ShowHealthbar()
        {
            var canvas = healthbarInstance.transform.Find("Canvas")
                                          .gameObject.GetComponent<Canvas>();

            var background = canvas.transform.Find("Border");
            background.gameObject.SetActive(true);
            var missinghealth = canvas.transform.Find("MissingHealth");
            missinghealth.gameObject.SetActive(true);
            var currenthealth = canvas.transform.Find("CurrentHealth");
            currenthealth.gameObject.SetActive(true);

            Debug.Log($"Pointer entered {name}");
        }

        public void HideHealthbar()
        {
            var canvas = healthbarInstance.transform.Find("Canvas")
                                          .gameObject.GetComponent<Canvas>();

            var background = canvas.transform.Find("Border");
            background.gameObject.SetActive(false);
            var missinghealth = canvas.transform.Find("MissingHealth");
            missinghealth.gameObject.SetActive(false);
            var currenthealth = canvas.transform.Find("CurrentHealth");
            currenthealth.gameObject.SetActive(false);

            Debug.Log($"Pointer entered {name}");
        }

        public abstract float GetApproximateDamage(BaseSkill ability);

        public abstract string UseAbility(BaseSkill ability, BaseUnit target = null);

        public virtual void InitiativeBestimmen(double modifier = 1)
        {
            var initiative = (float)modifier * BaseInitiative;

            CurrentInitiative = initiative.InfuseRandomness();
        }

        public virtual void Initialize() => CurrentHitpoints = MaximumHitpoints;

        //BattleService kram hier her.
        //Bild zu Splash
        //Collider und Skript deaktivieren
        //Etc
        //GGF in die DealDamage Methode rein und Callen
        //Muss für jede Unit ausimplementiert werden?
        //Vermutlich beides ok -> Performance

        #region Stats

        public int Strength     { get; set; }
        public int Dexterity    { get; set; }
        public int Constitution { get; set; }
        public int Wisdom       { get; set; }
        public int Quickness    { get; set; }
        public int Intuition    { get; set; }
        public int Logic        { get; set; }
        public int Willpower    { get; set; }
        public int Charisma     { get; set; }

        #endregion
    }
}