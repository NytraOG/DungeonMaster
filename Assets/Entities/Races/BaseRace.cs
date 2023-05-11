using UnityEngine;

namespace Entities.Races
{
    public abstract class BaseRace : ScriptableObject
    {
        public Sprite sprite;

        public abstract void ApplyModifiers<T>(T unit)
                where T : BaseUnit;

        public abstract void ApplyAbilities<T>(T unit)
                where T : BaseUnit;
    }
}