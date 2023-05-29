using Equipment;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EquipmentSlotUi : MonoBehaviour
    {
        public  Slot                   slot;
        public  Image                  itemSprite;
        public  EquipmentSlot          assignedSlot;
        private Button                 button;
        public  StaticEquipmentDisplay ParentDisplay { get; set; }

        private void Awake()
        {
            ClearSlot();

            button = GetComponent<Button>();
            button.onClick.AddListener(OnUiSlotClick);

            ParentDisplay = transform.parent.transform.parent.GetComponent<StaticEquipmentDisplay>();
        }

        public void Initialize(EquipmentSlot equipmentSlot)
        {
            assignedSlot = equipmentSlot;

            UpdateUiSlot(equipmentSlot);
        }

        private void OnUiSlotClick() => ParentDisplay.SlotClicked();

        public void UpdateUiSlot(EquipmentSlot equipmentSlot)
        {
            if (equipmentSlot.itemData is not null)
            {
                itemSprite.sprite = equipmentSlot.itemData.icon;
                itemSprite.color  = Color.white;
            }
            else
                ClearSlot();
        }

        public void ClearSlot()
        {
            assignedSlot.ClearSlot();
            itemSprite.sprite = null;
            itemSprite.color  = Color.clear;
        }
    }
}