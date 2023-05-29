using System.Collections.Generic;
using Equipment;
using Inventory;
using Items;

namespace UI
{
    public class StaticEquipmentDisplay : InventoryDisplay
    {
        private EquipmentSystem                            equipmentSystem;
        private Dictionary<EquipmentSlotUi, EquipmentSlot> slotLerry = new();
        public  EquipmentSlotUi[]                          slots;

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
            equipmentSystem                        =  holder.equipmentSystem;
            equipmentSystem.OnEquipmentSlotChanged += UpdateSlot;
        }

        private void UpdateSlot(Slot updatedSlot)
        {
            foreach (var slot in slotLerry)
            {
                if (slot.Value.slot == updatedSlot)
                    slot.Key.UpdateUiSlot(slot.Value);
            }
        }

        public override void AssignSlot()
        {
            if (SlotAssigned || equipmentSystem is null)
                return;

            slotLerry = new Dictionary<EquipmentSlotUi, EquipmentSlot>();

            for (var i = 0; i < equipmentSystem.equipmentSlots.Length; i++)
            {
                slotLerry.Add(slots[i], equipmentSystem.equipmentSlots[i]);
                slots[i].Initialize(equipmentSystem.equipmentSlots[i]);
            }

            SlotAssigned = true;
        }
    }
}