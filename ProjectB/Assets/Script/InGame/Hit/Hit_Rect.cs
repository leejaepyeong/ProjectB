using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Rect : Hit
{
    private HitEvenet.Rect rect;
    private Vector2 curRange;
    private Vector2 preRange;

    public override void Init(HitBehavior hitBehavior, HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        base.Init(hitBehavior, hitEvent, caster, target);
        rect = hitData as HitEvenet.Rect;

        curRange = Vector2.zero;
        preRange = Vector2.zero;
    }

    protected override void ActiveWave()
    {
        if (curRange.x >= rect.Range.x)
        {
            isDone = true;
            return;
        }
        preRange = curRange;
        curRange += new Vector2(waveSpeed * deltaTime, (rect.Range.y/rect.Range.x) * waveSpeed * deltaTime);
        ApplyDamage();
    }

    protected override List<UnitBehavior> GetTargetList()
    {
        List<UnitBehavior> list = new();
        Collider2D[] col = null;
        if (isWave)
            col = Physics2D.OverlapBoxAll(startPos, curRange, 0, 12);
        else
            col = Physics2D.OverlapBoxAll(startPos, rect.Range, 12);

        for (int i = 0; i < col.Length; i++)
        {
            UnitBehavior unit = col[i].GetComponentInParent<UnitBehavior>();
            if (unit == null || unit.UnitState.isDead || hittedDic.ContainsKey(unit.ID)) continue;
            list.Add(unit);
        }

        return list;
    }
}
