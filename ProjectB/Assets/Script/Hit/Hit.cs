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
    protected Vector2 startPosOffset;
    protected Vector3 startRotOffset;

    public virtual void Init(HitBehavior hitBehavior, HitEvent hitEvent, UnitBehavior caster, UnitBehavior target)
    {
        this.hitBehavior = hitBehavior;
        this.hitEvent = hitEvent;
        this.caster = caster;
        this.target = target;
        startPosOffset = hitEvent.startPos;
        startRotOffset = hitEvent.startRot;
    }
    public virtual void UpdateFrame(float deltaTime)
    {
        this.deltaTime = deltaTime;
        elaspedTime += deltaTime;

        if(isWave)
        {
            ActiveWave();
        }
        else
        {
            ApplyDamage();
        }
    }

    protected virtual void ActiveWave()
    {

    }

    protected virtual void ApplyDamage()
    {

    }
}
