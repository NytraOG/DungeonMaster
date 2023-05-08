using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
    public class InventoryItemData : ScriptableObject
    {
        public                  string id;
        public                  string displayName;
        public                  Sprite icon;
        public                  int    maxStackSize;
        public                  int    levelRequirement;
        [TextArea(2, 2)] public string description;
    }
}