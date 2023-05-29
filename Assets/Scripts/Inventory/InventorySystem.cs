using System;
using System.Linq;
using Items;
using UnityEngine.Events;

namespace Inventory
{
    [Serializable]
    public class InventorySystem
    {
        public InventorySlot[]            inventorySlots;
        public UnityAction<InventorySlot> OnInventorySlotChanged;

        public InventorySystem(int size)
        {
            inventorySlots = new InventorySlot[size];

            for (var i = 0; i < size; i++)
                inventorySlots[i] = new InventorySlot();
        }

        public bool AddToInventory(InventoryItemData itemToAdd, int amount)
        {
            if (itemToAdd is Consumable consumable)
            {
                var fillableSlots = inventorySlots.Where(s => s.itemData is not null &&
                                                              s.itemData.id == itemToAdd.id &&
                                                              s.CurrentStackSize + amount <= consumable.maxStackSize)
                                                  .ToList();

                if (fillableSlots.Any())
                {
                    var slot = fillableSlots[0];
                    slot.AddToStack(amount);

                    OnInventorySlotChanged?.Invoke(slot);

                    return true;
                }
            }

            var emptySlots = inventorySlots.Where(s => s.ItemData is null)
                                           .ToList();

            if (!emptySlots.Any())
                return false;

            var emptySlot = emptySlots[0];
            emptySlot.itemData = itemToAdd;
            emptySlot.AddToStack(amount);

            OnInventorySlotChanged?.Invoke(emptySlot);

            return true;
        }
    }
}