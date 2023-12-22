using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hit
{
    protected bool isDone;
    public bool IsDone => isDone;
    protected bool isWave;

    protected float elaspedTime;
    protected float deltaTime;
    protected HitEvent hitEvent;
    protected HitEvenet.IHitData hitData;

    protected HitBehavior hitBehavior;
    protected GameObject Model;
    protected Dictionary<int, UnitBehavior> hittedDic = new Dictionary<int, UnitBehavior>();

    public Transform transform => hitBehavior.transform;
    protected UnitBehavior caster;
    protected UnitBehavior target;
    protected Vector3 startPos;
    protected Quaternion startRot;
    protected Vector3 startPosOffset;
    protected Vector3 startRotOffset;
    protected float maxDistance;
    protected float curDistance;
    protected float preDistance;
    protected float waveSpeed;

    protected List<UnitBehavior> targetList = new();

    public virtual void Init(HitBehavior hitBehavior, HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        this.hitBehavior = hitBehavior;
        this.hitEvent = hitEvent;
        this.caster = caster;
        this.target = target;
        hitData = hitEvent.GetHitData();
        startPos = target == null ? caster.GetPos() : target.GetPos();
        startRot = target == null ? caster.GetRot() : target.GetRot();
        startPosOffset = hitEvent.startPos;
        startRotOffset = hitEvent.startRot;
        maxDistance = hitEvent.radius;
        curDistance = 0;
        preDistance = 0;
        waveSpeed = hitEvent.speed;

        hittedDic.Clear();
        targetList.Clear();
        isWave = hitEvent.hitType == HitEvenet.eHitType.Wave;
    }
    public virtual void UpdateFrame(float deltaTime)
    {
        if (isDone) return;
        this.deltaTime = deltaTime;
        elaspedTime += deltaTime;

        if(isWave)
        {
            ActiveWave();
        }
        else
        {
            ApplyDamage();
            isDone = true;
        }
    }

    protected virtual void ActiveWave()
    {
        if (curDistance >= maxDistance)
        {
            isDone = true;
            return;
        }
        preDistance = curDistance;
        curDistance += deltaTime * waveSpeed;
        ApplyDamage();
    }

    protected virtual void ApplyDamage()
    {
        var list = GetTargetList();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].UnitBase.ApplyDamage(caster, hitBehavior.skillInfo.skillRecord.damagePerValue, hitBehavior.skillInfo.skillRecord.damagePerType);
            hittedDic.Add(list[i].ID, list[i]);
        }
    }

    protected virtual List<UnitBehavior> GetTargetList()
    {
        return null;
    }
}
