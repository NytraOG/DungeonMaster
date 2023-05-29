using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = nameof(Equipment), menuName = "Item/Equipment")]
    public class Equipment : BaseItem
    {
        [Header("Armour")] public int slashNormal;
        public                    int slashGood;
        public                    int slashCritical;
        public                    int pierceNormal;
        public                    int pierceGood;
        public                    int pierceCritical;
        public                    int crushNormal;
        public                    int crushGood;
        public                    int crushCritical;
        public                    int fireNormal;
        public                    int fireGood;
        public                    int fireCritical;
        public                    int iceNormal;
        public                    int iceGood;
        public                    int iceCritical;
        public                    int lightningNormal;
        public                    int lightningGood;
        public                    int lightningCritical;
    }
}