using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehavior : BaseBehavior
{
    public SkillInfo skillInfo;
    private HitEvent hitEvent;
    private Hit hit;

    private UnitBehavior caster;
    private UnitBehavior target;
    public void Init(SkillInfo skillInfo, HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        this.skillInfo = skillInfo;
        this.hitEvent = hitEvent;
        this.caster = caster;
        this.target = target;
        elaspedTime = 0;
        SpawnHitObj();
        isInit = true;
    }

    public void Close()
    {
        isInit = false;
        HitManager.Instance.RemoveHit(this);
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isInit == false) return;
        if (hit.IsDone) Close();
        base.UpdateFrame(deltaTime);

        hit.UpdateFrame(deltaTime);
    }

    private void SpawnHitObj()
    {
        switch (hitEvent.hitRange)
        {
            case HitEvenet.eHitRange.Circle:
                hit = Utilities.StaticeObjectPool.Pop<Hit_Circle>();
                break;
        }

        hit.Init(this, hitEvent, caster, target);
    }
}
