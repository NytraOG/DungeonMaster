using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Enemies;
using UnityEngine;

namespace Battlefield
{
    public class BattleService : MonoBehaviour
    {
        public  GameObject       healthbarPrefab;
        public  List<BaseFoe>    enemies;
        public  List<GameObject> heroes;
        public  List<GameObject> enemiesToSpawn = new();
        private List<BaseUnit>   combatants     = new();

        public void Start() => InitScene();

        private void Update() { }

        public void ToggleHealthbars()
        {
            
        }

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