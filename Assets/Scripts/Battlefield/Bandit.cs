using Entities;
using Entities.Enemies;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlefield
{
    public class Bandit : BaseFoe, IPointerClickHandler
    {
        public override float Schadensmodifier => 1f;

        // Start is called before the first frame update
        private void Start()
        {
            Intuition = 2;
            Charisma  = 1;
            Hitpoints = HitpointsMax;
            Schaden   = 3;
        }

        // Update is called once per frame
        private void Update() { }

        public void OnPointerClick(PointerEventData eventData) => Debug.Log("Bandit clicker");

        public override int DealDamage(BaseUnit target)
        {
            var damageDealt = Schaden * Schadensmodifier;
            target.Hitpoints -= damageDealt;

            return (int)damageDealt;
        }
    }
}