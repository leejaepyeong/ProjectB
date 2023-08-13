using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile
{
    protected bool isDone;
    public bool IsDone => isDone;
    protected bool isHitEnd;
    protected float speed;
    protected float duration;
    protected float moveDistance;
    protected float elapsedDistance;
    protected float DeltaTime;
    protected float elapsedTime;
    protected ProjectileEvent projectileEvent;
    protected Projectiles.IProjectileData projectileData;

    protected ProjectileBehavior projectileBehavior;
    protected GameObject Model;
    protected Transform transform => projectileBehavior.transform;
    protected Transform caster;
    protected Transform targetTrf;
    protected Dictionary<int ,UnitBehavior> targetList = new();
    protected int hitCount;

    public virtual void Init(ProjectileBehavior projectileBehavior, ProjectileEvent projectileEvent, Transform caster, Transform targetTrf)
    {
        this.projectileBehavior = projectileBehavior;
        this.projectileEvent = projectileEvent;
        this.caster = caster;
        this.targetTrf = targetTrf;
        ResetData();
    }

    public virtual void ResetData()
    {
        isDone = false;
        isHitEnd = false;
        hitCount = 0;
        projectileData = projectileEvent.GetProjectileData();
        elapsedTime = 0;
        elapsedDistance = 0;

        targetList.Clear();
    }

    public virtual void DeInit()
    {

    }

    public virtual void Open()
    {

    }
    public virtual void Close()
    {
        isDone = true;
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        DeltaTime = deltaTime;
        elapsedTime += DeltaTime;
        Move();
        if (CheckStop())
            Close();
    }

    public abstract void Move();
    public abstract bool CheckStop();
    public virtual void OnHit(UnitBehavior unit)
    {
        if (isHitEnd) return;

        hitCount += 1;

        switch (projectileEvent.projectileHit)
        {
            case Projectiles.eProjectileHit.Normal:
                isHitEnd = true;
                break;
            case Projectiles.eProjectileHit.Penetration:
                if (projectileEvent.MaxHitCount != 0 && hitCount >= projectileEvent.MaxHitCount)
                    isHitEnd = true;
                ApplyDamage(unit);
                break;
            default:
                break;
        }
    }

    public void AddHitTarget(UnitBehavior unit)
    {
        if (targetList.ContainsKey(unit.ID)) return;
        targetList.Add(unit.ID, unit);
        OnHit(unit);
    }

    public void RemoveHitTarget(UnitBehavior unit)
    {
        targetList.Remove(unit.ID);
    }

    public void ApplyDamage(UnitBehavior unit)
    {

    }
}