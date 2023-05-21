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
                if (creature.skills.Any(s => s.displayName == skill.displayName))
                    continue;

                if(skill is SupportSkill supportSkill)
                    supportSkill.PopulateBuffs(creature);

                creature.skills.Add(skill);
            }
        }
    }
}