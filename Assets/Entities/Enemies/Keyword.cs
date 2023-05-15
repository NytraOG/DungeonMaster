using System.Collections.Generic;
using System.Linq;
using Skills;
using UnityEngine;

namespace Entities.Enemies
{
    [CreateAssetMenu(fileName = "Keyword")]
    public class Keyword : BaseUnitModifikator
    {
        public List<BaseSkill> skills;

        public void PopulateSkills(BaseUnit creature)
        {
            foreach (var skill in skills)
            {
                if (creature.skills.All(s => s.displayName != skill.displayName))
                    creature.skills.Add(skill);
            }
        }
    }
}