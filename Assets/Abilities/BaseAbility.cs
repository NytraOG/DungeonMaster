﻿using UnityEngine;

namespace Abilities
{
    public abstract class BaseAbility : ScriptableObject
    {
        public          Sprite sprite;
        public abstract string AbilityName { get; }

        public abstract void Initialize(GameObject obj);

        public abstract void TriggerAbility();
    }
}