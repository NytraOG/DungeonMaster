using System.Linq;
using Abilities;
using UnityEngine;

namespace Entities.Races
{
    [CreateAssetMenu(fileName = "Orc", menuName = "Races/Orc", order = 0)]
    public class Orc : BaseRace
    {
        public Sprite          sprite; 
        public Roar            roarAbility;
        public MagicResistance magicResistanceAbility;

        public override void ApplyModifiers<T>(T unit)
        {
            unit.Strength  += 2;
            unit.Wisdom    -= 1;
            unit.Logic     -= 2;
            unit.Willpower += 1;
        }

        public override void ApplyAbilities<T>(T unit)
        {
            if (unit.abilities.All(a => a.name != roarAbility.name))
                unit.abilities.Add(roarAbility);

            if (unit.abilities.All(a => a.name != magicResistanceAbility.name))
                unit.abilities.Add(magicResistanceAbility);
        }
    }
}