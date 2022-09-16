using Entities.Enums;
using UnityEngine;

namespace Entities
{
    public abstract class BaseUnit : Base                                          
    {
        public          string  Name                { get; }
        public          int     Angriffswurf        { get; protected set; }         // Müsste später durch Skills ersetzt werden
        public          int     Schaden             { get; protected set; }         // Wird später auf Waffen migriert (?)
        public abstract float   Schadensmodifier    { get; }

        public          int     BaseInitiative      => 2 * Intuition + Quickness;
        public          int     AktionenGesamt      { get; protected set; }
        public          int     AktionenAktuell     { get; set; }
        public float BaseMeleeDefense => 2 * Dexterity + Quickness;    
        public float BaseRangedDefense => 2 * Quickness + Dexterity;     
        public float BaseMagicDefense => 2 * Willpower + Wisdom;
        public float BaseSocialDefense => 2 * Logic + Charisma;
        public          float   Lebenspunkte        { get; set; }
        public float LebenspunkteMax => 3 * Constitution + 2 * Strength;
        public          int     Rüstung             { get; set; }                   // Wird später auf Rüstungen migriert
        public          int     CritDamage          { get; protected set; }         // Wird später auf Waffen migriert
        public int Mana => 2 * Wisdom + Logic;
        public          bool    IstKampfunfähig     { get; protected set; }
        public abstract Party   Party               { get; }
        public          Vector2 Position            { get; set; }

        public abstract void DealDamage(BaseUnit target);

        public virtual float InitiativeBestimmen(float modifier) => BaseInitiative * modifier;  // Hier müsste die Variable Base Initiative mit einer Zahl zwischen 0 und 2 multipliziert werden

        #region Stats

        public int Strength           { get; set; }
        public int Dexterity { get; set; }
        public int Constitution     { get; set; }
        public int Wisdom         { get; set; }
        public int Quickness    { get; set; }
        public int Intuition        { get; set; }
        public int Logic            { get; set; }
        public int Willpower     { get; set; }
        public int Charisma         { get; set; }

        #endregion
    }
}