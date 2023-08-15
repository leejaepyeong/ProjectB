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
    protected override void GetTargetList()
    {
        if(isWave)
        {

        }
        else
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(startPos, maxDistance, transform.forward);
            for (int i = 0; i < hits.Length; i++)
            {

            }
        }
    }
}
