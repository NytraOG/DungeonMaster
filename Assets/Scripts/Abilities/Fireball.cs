using Entities;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "Fireball", menuName = "Abilities/Fireball", order = 0)]
    public class Fireball : BaseAbility
    {
        public override string AbilityName => AbilityNames.Fireball;

        public override void Initialize(GameObject obj) => throw new System.NotImplementedException();

        public override int TriggerAbility(BaseUnit actor, BaseUnit target) => throw new System.NotImplementedException();
    }
}