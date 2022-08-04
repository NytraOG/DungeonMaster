using Entities.Enums;
using UnityEngine;

namespace Entities
{
    public abstract class BaseUnit : Base
    {
        public          string Name             { get; set; }
        public          int    Angriffswurf     { get; protected set; }
        public          int    Schaden          { get; protected set; }
        public abstract float  Schadensmodifier { get;  }
        public          float    Initiative       { get; protected set; }
        public          int    AktionenGesamt   { get; protected set; }
        public          int    AktionenAktuell  { get; set; }
        public          int    Parade           { get; protected set; }
        public          float  Lebenspunkte     { get; set; }
        public          float  LebenspunkteMax  { get; protected set; }
        public          int    Rüstung          { get; set; }
        public          int    CritDamage       { get; protected set; }
        public          int    Mana             { get; protected set; }
        public          bool   IstKampfunfähig  { get; protected set; }
        public abstract Party  Party            { get; }

        public abstract void DealDamage(BaseUnit target);

        public virtual void InitiativeBestimmen() => Initiative = 2 * Intuition + Schnelligkeit;

        public Vector2 Position { get; set; }
        
        #region Stats

        public float Stärke           { get; set; }
        public float Geschicklichkeit { get; set; }
        public float Konstitution     { get; set; }
        public float Weisheit      { get; set; }
        public float Schnelligkeit    { get; set; }
        public float Intuition        { get; set; }
        public float Logik            { get; set; }
        public float Willenskraft     { get; set; }
        public float Charisma         { get; set; }

        #endregion
    }
}