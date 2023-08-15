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

    protected Transform transform => hitBehavior.transform;
    protected UnitBehavior caster;
    protected UnitBehavior target;
    protected Vector3 startPos;
    protected Quaternion startRot;
    protected Vector3 startPosOffset;
    protected Vector3 startRotOffset;
    protected float maxDistance;
    protected float waveSpeed;

    protected List<UnitBehavior> targetList = new();

    public virtual void Init(HitBehavior hitBehavior, HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        this.hitBehavior = hitBehavior;
        this.hitEvent = hitEvent;
        this.caster = caster;
        this.target = target;
        startPos = target == null ? caster.GetPos() : target.GetPos();
        startRot = target == null ? caster.GetRot() : target.GetRot();
        startPosOffset = hitEvent.startPos;
        startRotOffset = hitEvent.startRot;
        maxDistance = hitEvent.radius;
        waveSpeed = hitEvent.speed;

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

    }

    protected virtual void ApplyDamage()
    {

    }

    protected virtual void GetTargetList()
    {

    }
}
