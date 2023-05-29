using System.Collections.Generic;
using Inventory;

namespace UI
{
    public class StaticInventoryDisplay : InventoryDisplay
    {
        private InventorySystem                            inventorySystem;
        private Dictionary<InventorySlotUi, InventorySlot> slotLerry = new();
        public  InventorySlotUi[]                          slots;

        protected void Update()
        {
            switch (SlotAssigned)
            {
                case true when holder is not null && HeroChanged:
                    SubscribeUpdateSlot();
                    SlotAssigned = false;
                    break;
                case false when holder is not null && HeroChanged:
                    SubscribeUpdateSlot();
                    break;
            }

            if (!SlotAssigned)
                AssignSlot();
        }

        private void SubscribeUpdateSlot()
        {
            inventorySystem                        =  holder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }

        private void UpdateSlot(InventorySlot updatedSlot)
        {
            foreach (var slot in slotLerry)
            {
                if (slot.Value == updatedSlot)
                    slot.Key.UpdateUiSlot(updatedSlot);
            }
        }

        public override void AssignSlot()
        {
            if (SlotAssigned || inventorySystem is null)
                return;

            slotLerry = new Dictionary<InventorySlotUi, InventorySlot>();

            for (var i = 0; i < inventorySystem.inventorySlots.Length; i++)
            {
                slotLerry.Add(slots[i], inventorySystem.inventorySlots[i]);
                slots[i].Initialize(inventorySystem.inventorySlots[i]);
            }

            SlotAssigned = true;
        }
    }
}