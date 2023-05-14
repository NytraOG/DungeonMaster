using UnityEngine;

namespace Entities.Enemies.Monstertypes
{
    public abstract class BaseMonstertype : ScriptableObject
    {
        public string displayname;
        public int    strength;
        public int    constitution;
        public int    dexterity;
        public int    quickness;
        public int    intuition;
        public int    logic;
        public int    wisdom;
        public int    willpower;
        public int    charisma;
        public Sprite sprite;
        public int    health;
        public float  maximumHealth;
        public float  BaseMeleeDefense  => 2 * dexterity + quickness;
        public float  BaseRangedDefense => 2 * quickness + dexterity;
        public float  BaseMagicDefense  => 2 * willpower + wisdom;
        public float  BaseSocialDefense => 2 * logic + charisma;
        public int    BaseInitiative    => 2 * intuition + quickness;

        public void ApplyValues(Creature creature)
        {
            creature.Strength     = strength;
            creature.Constitution = constitution;
            creature.Dexterity    = dexterity;
            creature.Quickness    = quickness;
            creature.Intuition    = intuition;
            creature.Logic        = logic;
            creature.Wisdom       = wisdom;
            creature.Willpower    = willpower;
            creature.Charisma     = charisma;
        }
    }
}