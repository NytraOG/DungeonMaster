using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Classes;
using Entities.Enemies;
using Entities.Enums;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class BattleService : MonoBehaviour
    {
        public  GameObject       healthbarPrefab;
        public  GameObject       damageTextPrefab;
        public  List<BaseFoe>    enemies;
        public  List<GameObject> heroes;
        public  List<GameObject> enemiesToSpawn = new();
        private List<BaseUnit>   combatants     = new();
        private Random           rng;
        private Random           Rng => rng ??= new Random();

        public void Start() => InitScene();

        private void Update() { }

        private void InitScene()
        {
            var banditTransforms = new[]
            {
                new Vector3(1.78f, 1.38f),
                new Vector3(2.86f, 1.94f)
            };

            for (var i = 0; i < enemiesToSpawn.Count; i++)
            {
                var enemyGameobject = Instantiate(enemiesToSpawn[i], banditTransforms[i], Quaternion.identity);
                var enemy           = enemyGameobject.GetComponent<Bandit>();
                var healthbar       = Instantiate(healthbarPrefab, enemy.transform);

                healthbar.GetComponent<HealthpointBar>().unit = enemy;

                var canvas = healthbar.transform.Find("Canvas")
                                      .gameObject.GetComponent<Canvas>();

                var background    = canvas.transform.Find("Border");
                var missinghealth = canvas.transform.Find("MissingHealth");
                var currenthealth = canvas.transform.Find("CurrentHealth");

                var enemyposition = enemy.gameObject.transform.position;

                var enemyWidth  = enemy.GetComponent<RectTransform>().rect.width;
                var enemyHeight = enemy.GetComponent<RectTransform>().rect.height;

                background.transform.position    = new Vector3(enemyposition.x * 100 + 960 + enemyWidth / 2, enemyposition.y * 100 + 540 + enemyHeight, enemyposition.z);
                missinghealth.transform.position = new Vector3(enemyposition.x * 100 + 960 + enemyWidth / 2, enemyposition.y * 100 + 540 + enemyHeight, enemyposition.z);
                currenthealth.transform.position = new Vector3(enemyposition.x * 100 + 960 + enemyWidth / 2, enemyposition.y * 100 + 540 + enemyHeight, enemyposition.z);

                enemies.Add(enemy);
            }
        }

        public void KampfrundeAbhandeln() => InitiativereihenfolgeBestimmen();
        //StartCoroutine(MachBattleRoundShit());
        // private IEnumerator MachBattleRoundShit()
        // {
        //     foreach (var combatant in combatants)
        //     {
        //         if (!combatant.IstKampfunfähig)
        //         {
        //             var target = GetTargetForCombatant(combatant);
        //
        //             if (target == null)
        //             {
        //                 InstantiateFloatingCombatText(combatant, "No suitable Target");
        //                 continue;
        //             }
        //
        //             var damage = combatant.DealDamage(target);
        //             InstantiateFloatingCombatText(target, damage);
        //         }
        //
        //         yield return new WaitForSeconds(0.5f);
        //     }
        // }

        private BaseUnit GetTargetForCombatant(BaseUnit combatant)
        {
            if (combatant is BaseHero)
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

                c.InitiativeBestimmen();
            });

            combatants = combatants.OrderByDescending(u => u.CurrentInitiative)
                                   .ToList();
        }
    }
}