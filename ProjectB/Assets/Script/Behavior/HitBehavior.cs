using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehavior : MonoBehaviour
{
    private float elaspedTime;
    private HitEvent hitEvent;
    private Hit hit;

    private UnitBehavior caster;
    private UnitBehavior target;
    protected bool isInit;
    public bool IsInit => isInit;

    public void Init(HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
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

    public void UpdateFrame(float deltaTime)
    {
        if (isInit == false) return;
        if (hit.IsDone) Close();

        elaspedTime += deltaTime;
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

    }
}
