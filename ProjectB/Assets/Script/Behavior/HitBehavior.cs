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
    public void Init(HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        this.skillInfo = hitEvent.SkillInfo;
        this.hitEvent = hitEvent;
        this.caster = caster;
        this.target = target;
        elaspedTime = 0;
        StartTransform();
        SpawnHitObj();
        isInit = true;
    }

    private void StartTransform()
    {
        if (hitEvent.SkillInfo.skillRecord.targetType == eSkillTarget.Click_Target)
        {
            transform.position = hitEvent.SkillInfo.targetPos;
            transform.rotation = Quaternion.identity;
        }
        else if (hitEvent.SkillInfo.skillRecord.targetType == eSkillTarget.Click_Direction)
        {
            transform.position = target == null ? caster.GetPos() : target.GetPos();
            transform.rotation = Quaternion.LookRotation(hitEvent.SkillInfo.targetPos);
        }
        else
        {
            transform.position = target == null ? caster.GetPos() : target.GetPos();
            transform.rotation = target == null ? caster.GetRot() : target.GetRot();
        }
    }

    public void Close()
    {
        isInit = false;
        HitManager.Instance.RemoveHit(this);
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isInit == false) return;
        if (hit == null) return;
        if (hit.IsDone) Close();
        base.UpdateFrame(deltaTime);

        hit.UpdateFrame(deltaTime);
    }

    private void SpawnHitObj()
    {
        if (ProjectileManager.Instance.GameObjectPool.TryGet(hitEvent.hitEffect, out var model) == false) return;
        Model = model;
        Model.transform.SetParent(scaleTransform.transform);
        Model.transform.localPosition = Vector3.zero;

        switch (hitEvent.hitRange)
        {
            case HitEvenet.eHitRange.Circle:
                hit = Utilities.StaticeObjectPool.Pop<Hit_Circle>();
                break;
            case HitEvenet.eHitRange.Rect:
                hit = Utilities.StaticeObjectPool.Pop<Hit_Rect>();
                break;
            case HitEvenet.eHitRange.FanShape:
                hit = Utilities.StaticeObjectPool.Pop<Hit_FanShape>();
                break;
        }

        hit.Init(this, hitEvent, caster, target);
    }
}
