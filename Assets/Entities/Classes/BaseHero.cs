﻿using Entities.Enums;
using Inventory;
using Skills.neu;
using UnityEngine.Events;

namespace Entities.Classes
{
    public abstract class BaseHero : BaseUnit
    {
        public static   UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;
        public          BaseSkill                    inherentSkill;
        public          int                          inventorySize;
        public          InventorySystem              inventorySystem;
        public override Party                        Party           => Party.Ally;
        public          InventorySystem              InventorySystem => inventorySystem;
    }
}