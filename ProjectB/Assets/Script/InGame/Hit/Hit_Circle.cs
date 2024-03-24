using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Circle : Hit
{
    protected override List<UnitBehavior> GetTargetList()
    {
        List<UnitBehavior> list = new();
        Collider2D[] col = null;
        if (isWave)
            col = Physics2D.OverlapCircleAll(startPos, curDistance, Define.GetTargetLayer(caster.UnitState.team));
        else
            col = Physics2D.OverlapCircleAll(startPos, maxDistance, Define.GetTargetLayer(caster.UnitState.team));

        for (int i = 0; i < col.Length; i++)
        {
            UnitBehavior unit = col[i].GetComponentInParent<UnitBehavior>();
            if (unit == null || unit.UnitState.isDead || hittedDic.ContainsKey(unit.ID)) continue;
            list.Add(unit);
        }

        return list;
    }
}
