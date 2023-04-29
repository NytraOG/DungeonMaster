using System;
using System.Linq;
using Abilities;
using Entities.Enums;
using UnityEngine;
using Random = System.Random;

namespace Entities.Enemies
{
    public abstract class BaseFoe : BaseUnit
    {
        public override Party Party => Party.Foe;

        private void FixedUpdate()
        {
            if(abilities.All(a => a.AbilityName != AbilityNames.Shiv))
                abilities.Add(ScriptableObject.CreateInstance<Shiv>());
        }

        public void PickAbility()
        {
            if (!abilities.Any())
                return;

            var rnadomNoKek = new Random().Next(0, abilities.Count);

            SelectedAbility = abilities[rnadomNoKek];
        }
    }
}