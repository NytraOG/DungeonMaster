﻿using System.Collections.Generic;
using System.Linq;
using Abilities;
using Entities.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities
{
    public abstract class BaseUnit : Base
    {
        public          List<BaseAbility> abilities = new();
        public          BaseAbility       SelectedAbility      { get; set; }
        public          string            Name                 { get; }
        public          int               Angriffswurf         { get; protected set; } // Müsste später durch Skills ersetzt werden
        public          int               Schaden              { get; protected set; } // Wird später auf Waffen migriert (?)
        public abstract float             Schadensmodifier     { get; }
        public          int               BaseInitiative       => 2 * Intuition + Quickness;
        public          double            CurrentInitiative    { get; set; }
        public          int               AktionenGesamt       { get; protected set; }
        public          int               AktionenAktuell      { get; set; }
        public          float             BaseMeleeDefense     => 2 * Dexterity + Quickness;
        public          float             BaseRangedDefense    => 2 * Quickness + Dexterity;
        public          float             BaseMagicDefense     => 2 * Willpower + Wisdom;
        public          float             BaseSocialDefense    => 2 * Logic + Charisma;
        public          float             ManaregenerationRate { get; set; }
        public          float             MagicDefense         { get; set; }
        public          float             Hitpoints            { get; set; }
        public          float             MaximumHitpoints     { get; set; }
        public          int               Armour               { get; set; }           // Wird später auf Rüstungen migriert
        public          int               CritDamage           { get; protected set; } // Wird später auf Waffen migriert
        public          int               MaximumMana          { get; set; }
        public          int               CurrentMana          { get; set; }
        public          bool              IstKampfunfähig      => Hitpoints <= 0;
        public abstract Party             Party                { get; }
        public          Vector2           Position             { get; set; }

        private void Awake() => MagicDefense = BaseMagicDefense;

        private void Update()
        {
            if (Hitpoints <= 0)
                Die();
        }

        protected void SetInitialHitpointsAndMana()
        {
            MaximumMana      = 2 * Wisdom + Logic;
            CurrentMana      = MaximumMana;
            MaximumHitpoints = 3 * Constitution + 2 * Strength;
            Hitpoints        = MaximumHitpoints;
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
            panel.transform.Find("DmgValue").gameObject.GetComponent<TextMeshProUGUI>().text = Schaden.ToString();
            panel.transform.Find("HPValue").gameObject.GetComponent<TextMeshProUGUI>().text  = $"{(int)Hitpoints} / {(int)MaximumHitpoints}";

            panel.gameObject.SetActive(true);
        }

        public abstract int DealDamage(BaseUnit target);

        public abstract int UseAbility(BaseAbility ability, BaseUnit target = null);

        public virtual void InitiativeBestimmen(double modifier) => CurrentInitiative = BaseInitiative * modifier;

        public virtual void Initialize() => Hitpoints = MaximumHitpoints;

        private void Die()
        {
            //BattleService kram hier her.
            //Bild zu Splash
            //Collider und Skript deaktivieren
            //Etc

            //GGF in die DealDamage Methode rein und Callen
            //Muss für jede Unit ausimplementiert werden?
            //Vermutlich beides ok -> Performance
        }

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