using System;
using Items;

namespace Inventory
{
    [Serializable]
    public class InventorySlot
    {
        public InventoryItemData itemData;
        public int               currentStackSize;

        public InventorySlot(InventoryItemData source, int amount)
        {
            itemData         = source;
            currentStackSize = amount;
        }

        public InventorySlot() => ClearSlot();

        public InventoryItemData ItemData         => itemData;
        public int               CurrentStackSize => currentStackSize;

        // Start is called before the first frame update
        private void Start() { }

        // Update is called once per frame
        private void Update() { }

        public void AddToStack(int amount) => currentStackSize += amount;

        public void RemoveFromStack(int amount) => currentStackSize -= amount;

        public bool RoomLeftInStack(int amount, out int remainingStackSize)
        {
            remainingStackSize = itemData.maxStackSize - currentStackSize;

            return RoomLeftInStack(amount);
        }

        public bool RoomLeftInStack(int amount) => currentStackSize + amount <= itemData.maxStackSize;

        public void ClearSlot()
        {
            itemData         = null;
            currentStackSize = 0;
        }
    }
}