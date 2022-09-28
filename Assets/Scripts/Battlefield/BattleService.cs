using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Battlefield
{
    public class BattleService : MonoBehaviour, IPointerClickHandler
    {
        public  GameObject       damageTextPrefab;
        public  GameObject       heroInstance;
        public  List<GameObject> enemies;
        private List<BaseUnit>   combatants;
        private Random           rng;
        private Random           Rng => rng ??= new Random();

        public void Start()
        {
            combatants = new List<BaseUnit> { heroInstance.GetComponent<Hero>() };

            enemies.ForEach(e =>
            {
                combatants.Add(e.GetComponent<Skeleton>());
            });
        }

        private void Update()
        {
#if !DEBUG
            RandomFunHihi();
#endif
        }

        public void OnPointerClick(PointerEventData eventData) { }

        public void KampfrundeAbhandeln()
        {
            InitiativereihenfolgeBestimmen();
            
            
            combatants.ForEach(c =>
            {
                InstantiateFloatingCombatText(c, (int)c.CurrentInitiative);
            });
        }
        
        /* 
             * Spieler Kontrolle verbieten (clicks)
             * Höchste Ini macht Aktion
             *  Falls kampfunfähig, aktion skippen
             * Dann der reihe nach
             * Wenn keiner mehr eine Aktion hat Kontrolle zurückgeben
             */

        private void InstantiateFloatingCombatText(BaseUnit unitInstance, int damageDealt)
        {
            var damageTextInstance = Instantiate(damageTextPrefab, unitInstance.transform);

            var textcomponent = damageTextInstance.transform
                                                  .GetChild(0)
                                                  .GetComponent<TextMeshPro>();

            textcomponent.SetText(damageDealt.ToString());
        }

        private void InitiativereihenfolgeBestimmen()
        {
            combatants.ForEach(c =>
            {
                var modifier = Rng.NextDouble() * 2.0;
                c.InitiativeBestimmen(modifier);
            });

            combatants = combatants.OrderByDescending(u => u.CurrentInitiative)
                                   .ToList();
        }
    }
}