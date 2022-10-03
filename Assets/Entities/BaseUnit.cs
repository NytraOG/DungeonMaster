﻿using System;
using System.Linq;
using Entities.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities
{
    public abstract class BaseUnit : Base
    {
        public          string  Name              { get; }
        public          int     Angriffswurf      { get; protected set; } // Müsste später durch Skills ersetzt werden
        public          int     Schaden           { get; protected set; } // Wird später auf Waffen migriert (?)
        public abstract float   Schadensmodifier  { get; }
        public          int     BaseInitiative    => 2 * Intuition + Quickness;
        public          double  CurrentInitiative { get; set; }
        public          int     AktionenGesamt    { get; protected set; }
        public          int     AktionenAktuell   { get; set; }
        public          float   BaseMeleeDefense  => 2 * Dexterity + Quickness;
        public          float   BaseRangedDefense => 2 * Quickness + Dexterity;
        public          float   BaseMagicDefense  => 2 * Willpower + Wisdom;
        public          float   BaseSocialDefense => 2 * Logic + Charisma;
        public          float   Hitpoints         { get; set; }
        public          float   HitpointsMax      => 3 * Constitution + 2 * Strength;
        public          int     Armour            { get; set; }           // Wird später auf Rüstungen migriert
        public          int     CritDamage        { get; protected set; } // Wird später auf Waffen migriert
        public          int     Mana              => 2 * Wisdom + Logic;
        public          bool    IstKampfunfähig   => Hitpoints <= 0;
        public abstract Party   Party             { get; }
        public          Vector2 Position          { get; set; }

        public abstract int DealDamage(BaseUnit target);

        public virtual void InitiativeBestimmen(double modifier) => CurrentInitiative = BaseInitiative * modifier;

        public virtual void Initialize() => Hitpoints = HitpointsMax;

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

        private void OnMouseDown()
        {
            Debug.Log($"I clicked on {this.name}");

            var panel = SceneManager.GetActiveScene()
                                    .GetRootGameObjects()
                                    .FirstOrDefault(x => x.name == "Battlefield")
                                    .transform.Find("UI")
                                    .transform.Find("Canvas")
                                    .transform.Find("Panel");

            panel.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text     = this.name;
            panel.transform.Find("StrValue").gameObject.GetComponent<TextMeshProUGUI>().text = this.Strength.ToString();
            panel.transform.Find("DexValue").gameObject.GetComponent<TextMeshProUGUI>().text = this.Dexterity.ToString();
            panel.transform.Find("DmgValue").gameObject.GetComponent<TextMeshProUGUI>().text = this.Schaden.ToString();
            panel.transform.Find("HPValue").gameObject.GetComponent<TextMeshProUGUI>().text  = $"{this.Hitpoints} / {this.HitpointsMax}";
            panel.gameObject.SetActive(true);
        }
    }
}