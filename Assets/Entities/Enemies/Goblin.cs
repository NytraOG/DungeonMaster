﻿using Abilities;

namespace Entities.Enemies
{
    public sealed class Goblin : BaseFoe
    {
        public Goblin()
        {
            Intuition = 1;
            Charisma  = 1;

            Hitpoints = 10;
            Schaden   = 4;

            InitiativeBestimmen(2);
        }

        public override float Schadensmodifier => 1f;

        public override float GetApproximateDamage(BaseAbility ability) => throw new System.NotImplementedException();

        public override int? UseAbility(BaseAbility ability, BaseUnit target = null) => throw new System.NotImplementedException();
    }
}