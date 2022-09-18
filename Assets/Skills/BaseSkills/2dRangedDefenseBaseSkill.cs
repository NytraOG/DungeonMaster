using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class RangedDefenseBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Ranged;
    public override skillSubCategory SubCategory => skillSubCategory.DefenseSkill;
    public int skillDefenseRoll { get; set; }
}
