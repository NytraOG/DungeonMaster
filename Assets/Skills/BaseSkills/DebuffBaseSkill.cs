using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class DebuffBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Debuff;
    public override skillSubCategory SubCategory => skillSubCategory.Debuff;
    

}
