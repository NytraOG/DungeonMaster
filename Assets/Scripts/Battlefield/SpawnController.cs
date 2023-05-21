using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Enemies;
using Entities.Enums;
using Entities.Hero;
using Items;
using UI;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Battlefield
{
    public class SpawnController : MonoBehaviour
    {
        public          List<Creature>                 enemies;
        public          List<GameObject>               heroes;
        public          Material                       defaultMaterial;
        public          List<GameObject>               enemiesToSpawn = new();
        public          InventoryItemData              itemData1;
        public          InventoryItemData              itemData2;
        public          InventoryItemData              itemData3;
        public          InventoryItemData              itemData4;
        public          GameObject                     openBagAudioSource;
        public          GameObject                     characterPanel;
        public readonly Dictionary<Positions, bool>    fieldslots = new();
        private         List<BaseUnit>                 combatants = new();
        private         Dictionary<Positions, Vector3> creaturePositions;
        public         bool                           inventoryShown;
        public          UnityAction<SpawnEventArgs>    OnCreateSpawned;

        public void Start() => InitScene();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                var controller = transform.parent
                                          .transform.Find("BattleController")
                                          .gameObject.GetComponent<BattleController>();

                if (controller.selectedHero is null)
                    return;

                inventoryShown = !inventoryShown;

                if (inventoryShown)
                {
                    var inventoryDisplay = characterPanel.transform.Find("InventoryPanel").GetComponent<StaticInventoryDisplay>();

                    if (controller.selectedHero is not null)
                        inventoryDisplay.ChangeHero(controller.selectedHero.GetComponent<Hero>());

                    PlayOpenBagSound();
                }

                characterPanel.SetActive(inventoryShown);
                characterPanel.transform.Find("InventoryPanel").gameObject.SetActive(inventoryShown);
            }

            if (Input.GetKeyDown(KeyCode.P))
                MachItemInInventoryRein(itemData1);
            else if (Input.GetKeyDown(KeyCode.O))
                MachItemInInventoryRein(itemData2);
            else if (Input.GetKeyDown(KeyCode.L))
                MachItemInInventoryRein(itemData3);
            else if (Input.GetKeyDown(KeyCode.K))
                MachItemInInventoryRein(itemData4);
        }

        private void MachItemInInventoryRein(InventoryItemData item)
        {
            var controller = transform.parent
                                      .transform.Find("BattleController")
                                      .gameObject.GetComponent<BattleController>();

            if (controller.selectedHero is null)
                return;

            controller.selectedHero.InventorySystem.AddToInventory(item, 1);
        }

        public void ResetBattle()
        {
            var controller = transform.parent
                                      .transform.Find("BattleController")
                                      .gameObject.GetComponent<BattleController>();

            enemies = new List<Creature>();

            foreach (var enemie in FindObjectsOfType<Creature>())
                Destroy(enemie.gameObject);

            InitScene();

            controller.AbilitySelection = new List<AbilitySelection>();
            controller.allesDa          = false;
        }

        public void SpawnCreatureRandomly()
        {
            var amount      = enemiesToSpawn.Count;
            var index       = new Random().Next(0, amount);
            var creature    = enemiesToSpawn[index];
            var keyToUpdate = Positions.None;

            foreach (var fieldslot in fieldslots)
            {
                if (fieldslot.Value)
                    continue;

                CreateCreature(creature, creaturePositions[fieldslot.Key]);

                keyToUpdate = fieldslot.Key;

                break;
            }

            if (fieldslots.ContainsKey(keyToUpdate))
                fieldslots[keyToUpdate] = true;

            OnCreateSpawned?.Invoke(new SpawnEventArgs
            {
                Creature = creature.GetComponent<Creature>(),
                Position = keyToUpdate
            });
        }

        public void SpawnCreatureAtPosition(GameObject creature, Positions freeFavouritePosition)
        {
            CreateCreature(creature, creaturePositions[freeFavouritePosition]);

            if (fieldslots.ContainsKey(freeFavouritePosition))
                fieldslots[freeFavouritePosition] = true;

            OnCreateSpawned?.Invoke(new SpawnEventArgs
            {
                Creature = creature.GetComponent<Creature>(),
                Position = freeFavouritePosition
            });
        }

        private void InitScene()
        {
            characterPanel.transform.Find("InventoryPanel").gameObject.SetActive(inventoryShown);
            characterPanel.SetActive(inventoryShown);

            creaturePositions = new Dictionary<Positions, Vector3>
            {
                { Positions.FrontMiddel, new Vector3(2.27f, 1.17f, 0) },
                { Positions.FrontLeft, new Vector3(3.242f, 1.632f, 0) },
                { Positions.FrontRight, new Vector3(1.632f, 0.748f, 0) },
                { Positions.LeftFlankMiddel, new Vector3(5.501f, 2.849f, 0) },
                { Positions.LeftFlankLeft, new Vector3(6.395f, 3.311f, 0) },
                { Positions.LeftFlankright, new Vector3(4.647f, 2.378f, 0) },
                { Positions.RightFlankMiddle, new Vector3(-0.793f, -0.617f, 0) },
                { Positions.RightFlankRight, new Vector3(-1.638f, -1.128f, 0) },
                { Positions.RightFlankLeft, new Vector3(0.013f, -0.156f, 0) },
                { Positions.CenterMiddle, new Vector3(0.994f, 1.964f, 0) },
                { Positions.CenterLeft, new Vector3(1.937f, 2.388f, 0) },
                { Positions.CenterRight, new Vector3(0.15f, 1.445f, 0) },
                { Positions.BackMiddle, new Vector3(-0.239f, 2.662f, 0) },
                { Positions.BackLeft, new Vector3(0.553f, 3.065f, 0) },
                { Positions.BackRight, new Vector3(-1.231f, 2.133f, 0) }
            };

            foreach (var position in creaturePositions)
                fieldslots.Add(position.Key, false);

            for (var i = 0; i < enemiesToSpawn.Count; i++)
            {
                if (i == 0)
                    continue;

                CreateCreature(enemiesToSpawn[i], creaturePositions[(Positions)i]);

                fieldslots[(Positions)i] = true;
            }
        }

        private void CreateCreature(GameObject creature, Vector3 enemyTransform)
        {
            var enemyGameobject = Instantiate(creature, enemyTransform, Quaternion.identity);
            var enemy           = enemyGameobject.GetComponent<Creature>();
            enemyGameobject.GetComponent<SpriteRenderer>().material = defaultMaterial;

            enemies.Add(enemy);
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

            combatants = combatants.OrderByDescending(u => u.ModifiedInitiative)
                                   .ToList();
        }

        public struct SpawnEventArgs
        {
            public Creature  Creature { get; set; }
            public Positions Position { get; set; }
        }
    }
}