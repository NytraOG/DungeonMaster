using System;
using System.Collections.Generic;
using Battlefield;
using Entities;
using Entities.Enemies;
using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [Serializable]
    [CreateAssetMenu(fileName = "Summon Skill", menuName = "Skills/Summon")]
    public class SummonSkill : BaseSkill
    {
        public List<GameObject> spawnsPerCast = new();

        public override string Activate(BaseUnit actor, BaseUnit target)
        {
            foreach (var creature in spawnsPerCast)
            {
                var creatureScript = creature.GetComponent<Creature>();
                var service        = FindObjectOfType<SpawnController>();

                var freeFavouritePosition = Positions.None;

                foreach (var position in creatureScript.favouritePositions)
                {
                    var isOccupied = service.fieldslots[position];

                    if (isOccupied)
                        continue;

                    freeFavouritePosition = position;
                }

                if (freeFavouritePosition != Positions.None)
                    service.SpawnCreatureAtPosition(creature, freeFavouritePosition);
            }

            return string.Empty;
        }
    }
}