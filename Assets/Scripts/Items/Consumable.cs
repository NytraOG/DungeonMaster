using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = nameof(Consumable), menuName = "Item/Consumable")]
    public class Consumable : BaseItem
    {
        public                     float apCostMultiplier = 1f;
        public                     int   maxCharges       = 1;
        public                     int   maxStackSize;
        [Header("Potions")] public int   addedHealing;
    }
}