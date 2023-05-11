using System.Linq;
using Skills;
using UnityEngine;

namespace Entities.Classes
{
    [CreateAssetMenu(fileName = "Mage", menuName = "Classes/Mage", order = 0)]
    public class Mage : BaseClass
    {
        public BaseSkill ability1;

        public override void ApplyModifiers<T>(T unit)
        {
            unit.Angriffswurf  *= 1.15f;
            unit.SocialDefense *= 0.95f;
            //Melee senken
            unit.MaximumHitpoints *= 0.8f;
            unit.MaximumMana      *= 1.4f;
        }

        public override void ApplyAbilities<T>(T unit)
        {
            if (unit.skills.All(a => a.name != ability1.name))
                unit.skills.Add(ability1);
        }
    }
}