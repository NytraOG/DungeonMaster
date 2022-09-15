using System;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class BattleService : MonoBehaviour
    {
        public GameObject damageTextPrefab, enemyInstance, heroInstance;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                InstantiateFloatingCombatText(enemyInstance);
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                InstantiateFloatingCombatText(heroInstance);
            }
        }

        private void InstantiateFloatingCombatText(GameObject unitInstance)
        {
            var rng                = new Random();
            var dmg                = rng.Next(0, 9999);
            var damageTextInstance = Instantiate(damageTextPrefab, unitInstance.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(dmg.ToString());
        }
    }
}