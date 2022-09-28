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

        // Start is called before the first frame update
        private void Start()
        {
            gameObject.AddComponent<Assassin>();
            gameObject.AddComponent<Goblin>();
        }

        // Update is called once per frame
        private void Update()
        {
            var gobbo      = GetComponent<Goblin>();
            var ass        = GetComponent<Assassin>();
            gobbo.InitiativeBestimmen(5);

            if (ass is not null && ass.Hitpoints <= 0)
            {
                Debug.Log("Assassin is ded");
                Destroy(ass);
                ally.enabled             = false;
                allyAttackButton.enabled = false;
            }

            if (gobbo is not null && gobbo.Hitpoints <= 0)
            {
                Debug.Log("Gobbo is ded");
                Destroy(gobbo);
                foe.enabled             = false;
                foeAttackButton.enabled = false;
            }
        }

        public void FoeAttack()
        {
            var gobbo = GetComponent<Goblin>();
            var ass   = GetComponent<Assassin>();

            if (gobbo is not null && ass is not null)
                gobbo.DealDamage(ass);
        }

        public void AllyAttack()
        {
            var gobbo = GetComponent<Goblin>();
            var ass   = GetComponent<Assassin>();

            if (gobbo is not null && ass is not null)
                ass.DealDamage(gobbo);
        }

        public void BackToMenu() => SceneManager.LoadScene(mainMenuScene);

        #region Field

        public SpriteRenderer foe;
        public SpriteRenderer ally;
        public SpriteRenderer allyFront;
        public SpriteRenderer allyLeft;
        public SpriteRenderer allyRight;
        public SpriteRenderer allyMiddle;
        public SpriteRenderer allyBack;
        public SpriteRenderer allyAmbush;
        public SpriteRenderer foeFront;
        public SpriteRenderer foeRight;
        public SpriteRenderer foeLeft;
        public SpriteRenderer foeMiddle;
        public SpriteRenderer foeBack;
        public SpriteRenderer foeAmbush;
        public Button         foeAttackButton;
        public Button         allyAttackButton;

        #endregion
    }
}