using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Player : Unit_Base
{
    public override void Init(UnitBehavior behavior)
    {
        base.Init(behavior);
        for (int i = 0; i < (int)eStat.END; i++)
        {
            unitState.SetStat((eStat)i, Manager.Instance.CurScene.isTestScene);
        }
    }

    public override void SearchTarget()
    {

    }
}
