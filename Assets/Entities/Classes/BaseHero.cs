using Abilities;
using Entities.Enums;
using Inventory;
using UnityEngine.Events;

namespace Entities.Classes
{
    public abstract class BaseHero : BaseUnit
    {
        public static   UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;
        public          BaseAbility                  inherentAbility;
        public          int                          inventorySize;
        public          InventorySystem              inventorySystem;
        public override Party                        Party           => Party.Ally;
        public          InventorySystem              InventorySystem => inventorySystem;
    }
}