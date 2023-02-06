using Entities.Classes;
using Entities.Enemies;
using UnityEngine;

namespace Battlefield
{
    public class BattleHandler : MonoBehaviour
    {
        [SerializeField] private BaseHero baseHero;
        [SerializeField] private BaseFoe  baseFoe;
    
        void Start()
        {
            SpawnHero();
            SpawnFoe();
        }

        void Update()
        {
        
        }

        private void SpawnHero()
        {
            Instantiate(baseHero, new Vector3(0, 0), Quaternion.identity);
        }

        private void SpawnFoe()
        {
            Instantiate(baseFoe, new Vector3(0, 3), Quaternion.identity);
        }
    }
}
