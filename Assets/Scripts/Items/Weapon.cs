using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = nameof(Weapon), menuName = "Item/Weapon")]
    public class Weapon : BaseItem
    {
        public                          float         apCostMultiplier = 1f;
        public                          DamageType    damagetype;
        public                          List<Keyword> neededConsumableKeyword;
        [Header("Added Damage")] public int           normal;
        public                          int           good;
        public                          int           critical;
    }
}