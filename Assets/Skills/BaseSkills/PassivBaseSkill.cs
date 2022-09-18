using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class PassivBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Passiv;
    public override skillSubCategory SubCategory => skillSubCategory.Buff;
    

}
