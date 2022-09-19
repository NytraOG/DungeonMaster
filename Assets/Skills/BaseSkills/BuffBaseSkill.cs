using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BuffBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Buff;
    public override skillSubCategory SubCategory => skillSubCategory.Buff;
    

}
