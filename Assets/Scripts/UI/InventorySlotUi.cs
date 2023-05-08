using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlotUi : MonoBehaviour
    {
        public  Image            itemSprite;
        public  TextMeshProUGUI  itemCount;
        public  InventorySlot    assignedSlot;
        private Button           button;
        public  InventoryDisplay ParentDisplay { get; private set; }

        private void Awake()
        {
            ClearSlot();

            button = GetComponent<Button>();
            button.onClick.AddListener(OnUiSlotClick);

            ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
        }

        public void Initialize(InventorySlot slot)
        {
            assignedSlot = slot;

            UpdateUiSlot(slot);
        }

        public void UpdateUiSlot(InventorySlot slot)
        {
            if (slot.ItemData is not null)
            {
                itemSprite.sprite = slot.ItemData.icon;
                itemSprite.color  = Color.white;
            }
            else
                ClearSlot();

            if (slot.CurrentStackSize > 1)
                itemCount.text = slot.CurrentStackSize.ToString();
        }

        public void UpdateUiSlot()
        {
            if (assignedSlot is not null)
                UpdateUiSlot(assignedSlot);
        }

        public void ClearSlot()
        {
            assignedSlot.ClearSlot();
            itemSprite.sprite = null;
            itemSprite.color  = Color.clear;
            itemCount.text    = string.Empty;
        }

        private void OnUiSlotClick() => ParentDisplay.SlotClicked(this);
    }
}