using System.Collections.Generic;
using Battlefield;
using Inventory;
using UnityEngine;

namespace UI
{
    public class StaticInventoryDisplay : InventoryDisplay
    {
        public  Hero              inventoryHolder;
        public  InventorySlotUi[] slots;
        public  bool              showInventory;
        private bool              heroChanged;
        private bool              slotAssigned;

        private void Awake() => gameObject.SetActive(showInventory);

        protected void Update()
        {
            switch (slotAssigned)
            {
                case true when inventoryHolder is not null && heroChanged:
                    SubscribeUpdateSlot();
                    slotAssigned = false;
                    break;
                case false when inventoryHolder is not null && heroChanged:
                    SubscribeUpdateSlot();
                    break;
            }

            if (!slotAssigned)
                AssignSlot();
        }

        private void SubscribeUpdateSlot()
        {
            inventorySystem                        =  inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }

        public void ChangeHero(Hero hero)
        {
            inventoryHolder = hero;

            heroChanged = true;
        }

        public override void AssignSlot()
        {
            if (slotAssigned || inventorySystem is null)
                return;

            slotLerry = new Dictionary<InventorySlotUi, InventorySlot>();

            for (var i = 0; i < inventorySystem.inventorySlots.Length; i++)
            {
                slotLerry.Add(slots[i], inventorySystem.inventorySlots[i]);
                slots[i].Initialize(inventorySystem.inventorySlots[i]);
            }

            Debug.Log("Slot assigned");

            slotAssigned = true;
        }
    }
}