using System;
using Entities;
using UnityEngine;

namespace Abilities
{
    public abstract class BaseAbility : ScriptableObject
    {
        public          Sprite sprite;
        public abstract string AbilityName { get; }

        public string GetDamageText(int damage) => damage == 0 ? string.Empty : $"Damage: <b>{damage}</b>{Environment.NewLine}{Environment.NewLine}";

        public abstract string GetTooltip(int damage = 0);

        public abstract float GetDamage(BaseUnit actor);

        public abstract void Initialize(GameObject obj);


        public abstract int TriggerAbility(BaseUnit actor, BaseUnit target);
    }
}