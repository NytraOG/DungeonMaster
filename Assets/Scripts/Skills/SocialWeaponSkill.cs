using Entities.Enums;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Social Weaponskill", menuName = "Skills/Social/Weapon")]
    public class SocialWeaponSkill : BaseSocialSkill
    {
        public override SkillCategory    Category    => SkillCategory.Social;
        public override SkillSubCategory SubCategory => SkillSubCategory.WeaponSkill;
    }
}