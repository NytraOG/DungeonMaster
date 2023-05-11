using System.Linq;
using UnityEngine;

namespace Entities.Classes
{
    [CreateAssetMenu(fileName = "Energist", menuName = "Classes/Energist", order = 0)]
    public class Energist : BaseClass
    {
        //public BaseAbility ability1;

        public override void ApplyModifiers<T>(T unit)
        {
            unit.Angriffswurf     *= 1.05f;
            unit.MeleeDefense     *= 1.1f;
            unit.SocialDefense    *= 0.85f;
            unit.MaximumHitpoints *= 1.2f;
            unit.MaximumMana      *= 0.9f;
        }

        public override void ApplyAbilities<T>(T unit)
        {
            // if (unit.abilities.All(a => a.name != ability1.name))
            //     unit.abilities.Add(ability1);
        }
    }
}