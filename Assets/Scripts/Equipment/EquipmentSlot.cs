using System;
using Items;

namespace Equipment
{
    [Serializable]
    public class EquipmentSlot
    {
        public InventoryItemData itemData;
        public Slot              slot;

        public EquipmentSlot(InventoryItemData source) => itemData = source;

        public EquipmentSlot() => ClearSlot();

        public void ClearSlot() => itemData = null;
    }
}