using Entities;
using Entities.Classes;
using Entities.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Battlefield
{
    public class BattlefieldMain : MonoBehaviour
    {
        public string mainMenuScene;

        #region Field

        public BattleService BattleService;
        
        public Spawn SpawnAllyMiddle;
        

        #endregion

        void Start()
        {
            var hero = BattleService.heroInstance;
            SetPosition(hero);

        }

        private void SetPosition(GameObject hero)
        {
            var spawnPos = SpawnAllyMiddle.BotMiddle.transform.position;
            
            hero.transform.position   = new Vector3(spawnPos.x, spawnPos.y);
            SpawnAllyMiddle.BotMiddle.Unit = hero.GetComponent<BaseUnit>();
        }

        public void BackToMenu() => SceneManager.LoadScene(mainMenuScene);

    }
}