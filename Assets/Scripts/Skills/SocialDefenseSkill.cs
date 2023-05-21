using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Social Defenseskill", menuName = "Skills/Social/Defense")]
    public class SocialDefenseSkill : BaseSocialSkill
    {
        public override SkillCategory    Category    => SkillCategory.Social;
        public override SkillSubCategory SubCategory => SkillSubCategory.DefenseSkill;
    }
}