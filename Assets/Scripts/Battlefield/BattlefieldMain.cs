using Entities;
using Entities.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Battlefield
{
    public class BattlefieldMain : MonoBehaviour
    {
        public string mainMenuScene;

        private void Start()
        {
            var hero = BattleService.heroInstance;
            SetPosition(hero, SpawnAllyMiddle.TopMiddle);
            var enemies = BattleService.enemies;

            SetPosition(enemies[0], SpawnFoeMiddle.TopLeft);
            SetPosition(enemies[1], SpawnFoeMiddle.TopRight);
            SetPosition(enemies[2], SpawnFoeMiddle.BotLeft);
            SetPosition(enemies[3], SpawnFoeMiddle.BotRight);
        }

        private void SetPosition<T>(T unit, Tile tile)
                where T : MonoBehaviour
        {
            var spawnPos = tile.transform.position;

            var isFoe = unit.GetComponent<BaseUnit>().Party == Party.Foe;
            unit.transform.position = isFoe ? new Vector3(spawnPos.x, spawnPos.y + 0.5f, -1) : new Vector3(spawnPos.x, spawnPos.y, -1);

            tile.unit = unit.GetComponent<BaseUnit>();
        }

        public void BackToMenu() => SceneManager.LoadScene(mainMenuScene);

        #region Field

        public BattleService BattleService;
        public Spawn         SpawnAllyMiddle;
        public Spawn         SpawnFoeMiddle;

        #endregion
    }
}