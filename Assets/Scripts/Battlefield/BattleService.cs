using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Enemies;
using Items;
using UI;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class BattleService : MonoBehaviour
    {
        public  GameObject        healthbarPrefab;
        public  List<BaseFoe>     enemies;
        public  List<GameObject>  heroes;
        public  List<GameObject>  enemiesToSpawn = new();
        public  InventoryItemData itemData1;
        public  InventoryItemData itemData2;
        public  InventoryItemData itemData3;
        public  GameObject        inventoryPanel;
        private List<BaseUnit>    combatants = new();
        private bool              inventoryShown;
        public  GameObject        openBagAudioSource;

        public void Start() => InitScene();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryShown = !inventoryShown;

                if (inventoryShown)
                {
                    var inventoryDisplay = inventoryPanel.GetComponent<StaticInventoryDisplay>();

                    var controller = transform.parent
                                              .transform.Find("BattleController")
                                              .gameObject.GetComponent<BattleController>();

                    if (controller.selectedHero is not null)
                        inventoryDisplay.ChangeHero(controller.selectedHero.GetComponent<Hero>());

                    PlayOpenBagSound();
                }

                inventoryPanel.SetActive(inventoryShown);
            }

            if (Input.GetKeyDown(KeyCode.P))
                MachItemInInventoryRein(itemData1);
            else if (Input.GetKeyDown(KeyCode.O))
                MachItemInInventoryRein(itemData2);
            else if (Input.GetKeyDown(KeyCode.L))
                MachItemInInventoryRein(itemData3);
        }

        private void MachItemInInventoryRein(InventoryItemData item)
        {
            var controller = transform.parent
                                      .transform.Find("BattleController")
                                      .gameObject.GetComponent<BattleController>();

            if (controller.selectedHero is null)
                return;

            controller.selectedHero.InventorySystem.AddToInventory(item, new Random().Next(0, 4));
        }

        public void ToggleHealthbars() { }

        public void ResetBattle()
        {
            var controller = transform.parent
                                      .transform.Find("BattleController")
                                      .gameObject.GetComponent<BattleController>();

            enemies = new List<BaseFoe>();

            foreach (var enemie in FindObjectsOfType<BaseFoe>())
                Destroy(enemie.gameObject);

            InitScene();

            controller.AbilitySelection = new List<AbilitySelection>();
            controller.allesDa          = false;
        }

        private void InitScene()
        {
            inventoryShown = inventoryPanel.GetComponent<StaticInventoryDisplay>().showInventory;

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

        private void PlayOpenBagSound()
        {
            var audiosource = openBagAudioSource.GetComponent<AudioSource>();

            audiosource.Play();
        }

        public void KampfrundeAbhandeln() => InitiativereihenfolgeBestimmen();

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