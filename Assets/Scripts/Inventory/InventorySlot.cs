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

        public void AddToStack(int amount) => currentStackSize += amount;

        public void RemoveFromStack(int amount) => currentStackSize -= amount;

        public bool RoomLeftInStack(int amount, out int remainingStackSize)
        {
            remainingStackSize = 0;

            if (itemData is Consumable consumable)
                remainingStackSize = consumable.maxStackSize - currentStackSize;

            return RoomLeftInStack(amount);
        }

        public bool RoomLeftInStack(int amount)
        {
            if (itemData is Consumable consumable)
                return currentStackSize + amount <= consumable.maxStackSize;

            return true;
        }

        public void ClearSlot()
        {
            itemData         = null;
            currentStackSize = 0;
        }
    }
}