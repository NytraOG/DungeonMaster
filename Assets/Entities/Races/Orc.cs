using System.Linq;
using Skills.neu;
using UnityEngine;

namespace Entities.Races
{
    [CreateAssetMenu(fileName = "Orc", menuName = "Races/Orc", order = 0)]
    public class Orc : BaseRace
    {
        public BaseSkill skill1;
        public BaseSkill skill2;

        public override void ApplyModifiers<T>(T unit)
        {
            unit.Strength  += 2;
            unit.Wisdom    -= 1;
            unit.Logic     -= 2;
            unit.Willpower += 1;
        }

        public override void ApplyAbilities<T>(T unit)
        {
            if (unit.skills.All(a => a.name != skill1.name))
                unit.skills.Add(skill1);

            if (unit.skills.All(a => a.name != skill2.name))
                unit.skills.Add(skill2);
        }
    }
}