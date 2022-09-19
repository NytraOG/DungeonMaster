using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlefield
{
    public class BattleService : MonoBehaviour, IPointerClickHandler
    {
        public GameObject damageTextPrefab, enemyInstance, heroInstance;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Camera.main is { } mainCam)
            {
                var mousePos   = mainCam.ScreenToWorldPoint(Input.mousePosition);
                var mousePos2D = new Vector2(mousePos.x, mousePos.y);

                var hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider is not null)
                    Debug.Log("Iwas getroffen");
            }

            // if (Camera.main != null)
            // {
            //     var hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);
            //     Debug.Log($"{hit.point.x} + {hit.point.y}");
            // }

            if (Input.GetKeyDown(KeyCode.X))
            {
                var hero        = heroInstance.gameObject.GetComponent<Hero>();
                var target      = enemyInstance.gameObject.GetComponent<Bandit>();
                var damageDealt = hero.DealDamage(target);

                InstantiateFloatingCombatText(enemyInstance, damageDealt, target.IstKampfunfähig);

                if (target.IstKampfunfähig)
                    Destroy(enemyInstance);
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                var bandit      = enemyInstance.gameObject.GetComponent<Bandit>();
                var target      = heroInstance.gameObject.GetComponent<Hero>();
                var damageDealt = bandit.DealDamage(target);

                InstantiateFloatingCombatText(heroInstance, damageDealt, target.IstKampfunfähig);

                if (target.IstKampfunfähig)
                    Destroy(heroInstance);
            }
        }

        public void OnPointerClick(PointerEventData eventData) { }

        private void InstantiateFloatingCombatText(GameObject unitInstance, int damageDealt, bool isDed)
        {
            var damageTextInstance = Instantiate(damageTextPrefab, unitInstance.transform);

            var textcomponent = damageTextInstance.transform
                                                  .GetChild(0)
                                                  .GetComponent<TextMeshPro>();

            textcomponent.SetText(damageDealt.ToString());

            if (isDed)
                textcomponent.color = Color.red;
        }

        private void Attack<TTarget, TAttacker>()
                where TTarget : BaseUnit
                where TAttacker : BaseUnit
        {
            var target   = GetComponent<TTarget>();
            var attacker = GetComponent<TAttacker>();

            if (target is not null && attacker is not null)
                attacker.DealDamage(target);
        }
    }
}