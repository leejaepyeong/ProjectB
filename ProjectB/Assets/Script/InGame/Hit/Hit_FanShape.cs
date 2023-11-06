using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_FanShape : Hit
{
    private HitEvenet.FanShape fanShape;
    public override void Init(HitBehavior hitBehavior, HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        base.Init(hitBehavior, hitEvent, caster, target);
        fanShape = hitData as HitEvenet.FanShape;
    }

    protected override List<UnitBehavior> GetTargetList()
    {
        List<UnitBehavior> list = new();
        Collider2D[] col = null;
        float range = maxDistance;
        if (isWave)
        {
            col = Physics2D.OverlapCircleAll(startPos, curDistance, 12);
            range = curDistance;
        }
        else
            col = Physics2D.OverlapCircleAll(startPos, maxDistance, 12);

        for (int i = 0; i < col.Length; i++)
        {
            UnitBehavior unit = col[i].GetComponentInParent<UnitBehavior>();
            if (unit == null || unit.UnitState.isDead || hittedDic.ContainsKey(unit.ID)) continue;
            if (CheckInRange(unit, range) == false) continue;
            list.Add(unit);
        }

        return list;
    }

    private bool CheckInRange(UnitBehavior target, float range)
    {
        Vector3 interV = target.GetPos() - startPos;
        if (Vector3.Distance(target.GetPos(), startPos) > range) return false;

        float degree = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(interV.normalized, hitBehavior.transform.forward));

        return degree < fanShape.Angle/2f;
    }
}
