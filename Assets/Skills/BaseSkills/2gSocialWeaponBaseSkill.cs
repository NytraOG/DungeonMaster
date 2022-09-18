using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class SocialWeaponBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Social;
    public override skillSubCategory SubCategory => skillSubCategory.WeaponSkill;
    public int skillAttackRoll { get; set; }
    public float skillDamageRoll { get; set; }  

}
