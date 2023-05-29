using System.Collections.Generic;
using Entities.Hero;
using UI;
using UnityEngine;

namespace Inventory
{
    public abstract class InventoryDisplay : MonoBehaviour
    {
        public    Hero                                       holder;
        public    MouseItemData                              mouseInventoryItem;
        protected bool                                       HeroChanged;
        protected bool                                       SlotAssigned;

        public abstract void AssignSlot();

        public void ChangeHero(Hero hero)
        {
            holder = hero;

            HeroChanged = true;
        }


        public void SlotClicked() => Debug.Log("Slot clicked");
    }
}