using Entities;
using Entities.Classes;
using Skills;
using Skills.BaseSkills;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class Hero : BaseHero
    {
        public          GameObject    skillKnives;
        private         Random        rng;
        private         BattleService service;
        public override float         Schadensmodifier => 1.25f;

        // Start is called before the first frame update
        private void Start()
        {
            Strength     =   5;
            Constitution =   3;
            Dexterity    =   9;
            Quickness    =   8;
            Intuition    =   8;
            Logic        =   5;
            Willpower    =   2;
            Wisdom       =   5;
            Charisma     =   1;
            Schaden      =   10;
            rng          =   new Random();
            Skills       =   new BaseSkill[4];
            service      =   battleService.GetComponent<BattleService>();
            Skills[0]    ??= Instantiate(skillKnives, gameObject.transform).GetComponent<Knives>();
        }

        // Update is called once per frame
        private void Update() { }

        private void OnMouseDown()
        {
            Debug.Log("kek");

            service.selectedHero = gameObject;
        }

        public override int DealDamage(BaseUnit target)
        {
            // rng = new Random();
            var modifier    = rng.Next(0, 2);
            var damageDealt = Schaden * Schadensmodifier * modifier;
            target.Hitpoints -= damageDealt;

            if (target.Hitpoints < 0)
                target.Hitpoints = 0;

            return (int)damageDealt;
        }
    }
}