using Entities;
using UnityEngine;

namespace Skills.neu
{
    public abstract class BaseSkill : ScriptableObject
    {
        public                  Sprite sprite;
        public                  string displayName;
        [TextArea(4, 4)] public string description;
        public                  float  scalingStrength;
        public                  float  scalingConstitution;
        public                  float  scalingDexterity;
        public                  float  scalingQuickness;
        public                  float  scalingIntuition;
        public                  float  scalingLogic;
        public                  float  scalingWillpower;
        public                  float  scalingWisdom;
        public                  float  scalingCharisma;
        public                  int    addedFlatDamage;
        public                  bool   appliesStun;

        public abstract void Activate(BaseUnit actor, BaseUnit target);
    }
}