using System.Linq;
using Abilities;
using UnityEngine;

namespace Entities.Races
{
    [CreateAssetMenu(fileName = "Neo Terran", menuName = "Races/Neo Terran", order = 0)]
    public class NeoTerran : BaseRace
    {
        public const float       ManaModifier     = 1.3f;
        public const float       HitpointModifier = 0.8f;
        public       BaseAbility ability1;

        public override void ApplyModifiers<T>(T unit)
        {
            unit.MaximumMana          =  (int)(unit.MaximumMana * ManaModifier);
            unit.MaximumHitpoints     =  (int)(unit.MaximumHitpoints * HitpointModifier);
            unit.ManaregenerationRate += 1; //TODO +20% Herolevel 
        }

        public override void ApplyAbilities<T>(T unit)
        {
            if (unit.abilities.All(a => a.name != ability1.name))
                unit.abilities.Add(ability1);
        }
    }
}