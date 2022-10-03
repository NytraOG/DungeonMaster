using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Classes;
using Entities.Enemies;
using Entities.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Battlefield
{
    public class BattleService : MonoBehaviour, IPointerClickHandler
    {
        public  GameObject       damageTextPrefab;
        public  GameObject       heroInstance;
        public  List<GameObject> enemies;
        private List<BaseUnit>   combatants;
        private List<BaseHero>   heroes;
        private Random           rng;
        private Random           Rng => rng ??= new Random();

        public void Start()
        {
            var hero = heroInstance.GetComponent<Hero>();
            heroes = new List<BaseHero>();
            heroes.Add(hero);
            hero.Initialize();

            combatants = new List<BaseUnit> { hero };

            enemies.ForEach(e =>
            {
                var enemyComponent = e.GetComponent<Skeleton>();
                enemyComponent.Initialize();
                combatants.Add(enemyComponent);
            });
        }

        private void Update()
        {
            StartCoroutine(CheckIfGameOver());
            
            combatants.ForEach(c =>
            {
                if (!c.IstKampfunfähig)
                    return;
                
                //Kampfunfähige Combatants sollten aus der Collection entfernt werden.
                //Aktuell haben Kampfunfähige Combatants noch eine Ini und verzögern den Combatflow.
                //Wie verhalten sich dann rezzes?

                var renderer = c.GetComponent<SpriteRenderer>();
                var sprite   = Resources.LoadAll<Sprite>("bloodPuddle").FirstOrDefault();
                renderer.sprite                      = sprite;
                c.enabled                            = false;
                c.GetComponent<Collider2D>().enabled = false;
            });
        }

        private IEnumerator CheckIfGameOver()
        {
            if (heroes.All(h => h.IstKampfunfähig))
            {
                yield return new WaitForSeconds(2);
                SceneManager.LoadScene("Scenes/Game Over", LoadSceneMode.Single);
            }
        }

        public void OnPointerClick(PointerEventData eventData) { }

        public void KampfrundeAbhandeln()
        {
            InitiativereihenfolgeBestimmen();
            StartCoroutine(MachBattleRoundShit());
        }

        private IEnumerator MachBattleRoundShit()
        {
            foreach (var combatant in combatants)
            {
                if (!combatant.IstKampfunfähig)
                {
                    var target = GetTargetForCombatant(combatant);

                    if (target == null)
                    {
                        InstantiateFloatingCombatText(combatant, "No suitable Target");
                        continue;
                    }
                    var damage = combatant.DealDamage(target);
                    InstantiateFloatingCombatText(target, damage);
                }

                yield return new WaitForSeconds(0.5f);
            }
            
        }

        private BaseUnit GetTargetForCombatant(BaseUnit combatant)
        {
            if(combatant is BaseHero)
                return combatants.FirstOrDefault(c => !c.IstKampfunfähig && c != combatant && c.Party == Party.Foe);

            return combatants.FirstOrDefault(c => !c.IstKampfunfähig && c != combatant && c is BaseHero);
        }

        /* 
             * Spieler Kontrolle verbieten (clicks)
             * Wenn keiner mehr eine Aktion hat Kontrolle zurückgeben
             */

        private void InstantiateFloatingCombatText(BaseUnit unitInstance, string combatText)
        {
            var textcomponent = CreateTextComponent(unitInstance);
            textcomponent.SetText(combatText);
        }

        private TextMeshPro CreateTextComponent(BaseUnit unitInstance)
        {
            var damageTextInstance = Instantiate(damageTextPrefab, unitInstance.transform);

            var textcomponent = damageTextInstance.transform
                                                  .GetChild(0)
                                                  .GetComponent<TextMeshPro>();

            return textcomponent;
        }

        private void InstantiateFloatingCombatText(BaseUnit unitInstance, int damageDealt)
        {
            var textcomponent = CreateTextComponent(unitInstance);

            if (unitInstance.Hitpoints <= 0)
            {
                textcomponent.color = new Color(255, 0, 0);
                textcomponent.SetText("Killing Blow!");
            }

            else
                textcomponent.SetText(damageDealt.ToString());
        }

        private void InitiativereihenfolgeBestimmen()
        {
            combatants.ForEach(c =>
            {
                if (c.IstKampfunfähig)
                    return;
                
                var modifier = Rng.NextDouble() * 2.0;
                c.InitiativeBestimmen(modifier);
            });

            combatants = combatants.OrderByDescending(u => u.CurrentInitiative)
                                   .ToList();
        }
    }
}