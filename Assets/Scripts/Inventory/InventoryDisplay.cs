using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Inventory
{
    public abstract class InventoryDisplay : MonoBehaviour
    {
        public    MouseItemData                              mouseInventoryItem;
        protected InventorySystem                            inventorySystem;
        protected Dictionary<InventorySlotUi, InventorySlot> slotLerry;
        public    InventorySystem                            InventorySystem => inventorySystem;
        public    Dictionary<InventorySlotUi, InventorySlot> SlotLerry       => slotLerry;

        public abstract void AssignSlot();

        protected virtual void UpdateSlot(InventorySlot updatedSlot)
        {
            foreach (var slot in slotLerry)
            {
                if (slot.Value == updatedSlot)
                    slot.Key.UpdateUiSlot(updatedSlot);
            }
        }

        public void SlotClicked(InventorySlotUi clickedSlot) => Debug.Log("Slot clicked");
    }
}