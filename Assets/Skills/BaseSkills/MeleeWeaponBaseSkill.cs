using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class MeleeWeaponBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Melee;
    public override skillSubCategory SubCategory => skillSubCategory.WeaponSkill;
    public override skillRange Range => skillRange.Melee;
    public int skillAttackRoll { get; set; }
    public int skillDefenseRoll{ get; set; }
    public float skillDamageRoll { get; set; }  

}
