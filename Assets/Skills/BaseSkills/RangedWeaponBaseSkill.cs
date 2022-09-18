using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class RangedWeaponBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Ranged;
    public override skillSubCategory SubCategory => skillSubCategory.WeaponSkill;
    public int skillAttackRoll { get; set; }
    public float skillDamageRoll { get; set; }  

}
