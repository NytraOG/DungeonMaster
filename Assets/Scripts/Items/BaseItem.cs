using System.Collections.Generic;
using Entities.Buffs;
using Entities.Enums;
using UnityEngine;

namespace Items
{
    public abstract class BaseItem : InventoryItemData
    {
        public                                   List<Keyword> keywords       = new();
        public                                   List<Buff>    appliedBuffsOnUsageOntoTarget;
        public                                   List<Debuff>  appliedDebuffsOnUsageOntoTarget;
        [Header("Effect")] public                string        effect = "TODO";
        public                                   string        boni   = "TODO";
        [Header("Requirements")] public          int           levelRequirement;
        public                                   Attribute     requiredAttribute;
        public                                   int           requiredLevelOfAttribute;
        [Header("Misc")] [TextArea(2, 2)] public string        fluff;
    }
}