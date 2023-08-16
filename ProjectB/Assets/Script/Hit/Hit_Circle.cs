using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Circle : Hit
{
    public override void Init(HitBehavior hitBehavior, HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        base.Init(hitBehavior, hitEvent, caster, target);
    }

    protected override void ActiveWave()
    {

    }
    protected List<UnitBehavior> GetTargetList()
    {
        List<UnitBehavior> list = new();

        if(isWave)
        {

        }
        else
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(startPos, maxDistance, 12);
            
            for (int i = 0; i < col.Length; i++)
            {
                UnitBehavior unit = col[i].GetComponentInParent<UnitBehavior>();
                if (unit == null || unit.UnitState.isDead) continue;
                list.Add(unit);
            }
        }

        return list;
    }
}
