using System.Reflection;
using Skills;
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

        public void CopyClassValues(BaseAbility sourceComp, BaseAbility targetComp)
        {
            var sourceFields = sourceComp.GetType()
                                         .GetFields(BindingFlags.Public |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Instance);

            foreach (var t in sourceFields)
            {
                var value = t.GetValue(sourceComp);
                t.SetValue(targetComp, value);
            }
        }
    }
}