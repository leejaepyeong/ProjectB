using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile
{
    protected bool isDone;
    public bool IsDone => isDone;
    protected float speed;
    protected float duration;
    protected float elasped;
    protected float moveDistance;
    protected float elapsedDistance;
    protected float DeltaTime;
    protected float elapsedTime;
    protected ProjectileEvent projectileEvent;
    protected Projectiles.IProjectileData projectileData;

    protected ProjectileBehavior projectileBehavior;
    protected Transform transform => projectileBehavior.transform;
    protected Transform caster;
    protected Transform targetTrf;
    protected int raycastHitCnt;
    protected RaycastHit[] raycastHits = new RaycastHit[30];
    protected Dictionary<int ,UnitBehavior> targetList = new();

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
        projectileData = projectileEvent.GetProjectileData();
        elapsedTime = 0;
        elapsedDistance = 0;

        targetList.Clear();
    }

    public virtual void UnInit()
    {

    }

    public virtual void Open()
    {

    }
    public virtual void Close()
    {

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
    
}
