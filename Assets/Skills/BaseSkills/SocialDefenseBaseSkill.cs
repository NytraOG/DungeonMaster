using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class SocialDefenseBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Social;
    public override skillSubCategory SubCategory => skillSubCategory.DefenseSkill;
    public int skillDefenseRoll { get; set; }

}
