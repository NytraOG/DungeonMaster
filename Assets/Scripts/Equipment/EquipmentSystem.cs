using System;
using System.Linq;
using Items;
using UnityEngine.Events;

namespace Equipment
{
    [Serializable]
    public class EquipmentSystem
    {
        public EquipmentSlot[]   equipmentSlots;
        public UnityAction<Slot> OnEquipmentSlotChanged;

        public EquipmentSystem() => equipmentSlots = new[]
        {
            new EquipmentSlot { slot = Slot.Helmet },
            new EquipmentSlot { slot = Slot.Amulet },
            new EquipmentSlot { slot = Slot.Shoulder },
            new EquipmentSlot { slot = Slot.Chest },
            new EquipmentSlot { slot = Slot.Belt },
            new EquipmentSlot { slot = Slot.Pants },
            new EquipmentSlot { slot = Slot.Boots },
            new EquipmentSlot { slot = Slot.Wrist },
            new EquipmentSlot { slot = Slot.Gloves },
            new EquipmentSlot { slot = Slot.Mainhand },
            new EquipmentSlot { slot = Slot.Offhand },
            new EquipmentSlot { slot = Slot.RingLeft },
            new EquipmentSlot { slot = Slot.RingRight },
            new EquipmentSlot { slot = Slot.QuickConsumable1 }
        };

        public void AddToSlot(BaseItem item, Slot slot)
        {
            var equipmentSlot        = equipmentSlots.First(s => s.slot == slot);
            var itemPreviouslyInSlot = equipmentSlot.itemData;

            equipmentSlot.ClearSlot();
            equipmentSlot.itemData = item;
        }
    }
}