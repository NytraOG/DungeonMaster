using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class DungeonBuffBaseSkill : BaseSkill
{
    public override skillCategory Category => skillCategory.Buff;
    public override skillSubCategory SubCategory => skillSubCategory.DungeonBuff;
    

}
