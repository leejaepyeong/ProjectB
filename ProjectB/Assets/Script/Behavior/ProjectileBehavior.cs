using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : BaseBehavior
{
    public SkillInfo skillInfo;

    private ProjectileEvent projectileEvent;
    private Projectile projectile;

    private UnitBehavior caster;
    private UnitBehavior target;

    public void Init(SkillInfo skillInfo, ProjectileEvent projectileEvent, UnitBehavior caster, UnitBehavior target)
    {
        this.skillInfo = skillInfo;
        this.projectileEvent = projectileEvent;
        this.caster = caster;
        this.target = target;
        elaspedTime = 0;
        SpawnProjectile();
        isInit = true;
    }

    public void Close()
    {
        isInit = false;
        ProjectileManager.Instance.RemoveProjectile(this);
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        projectile.UpdateFrame(deltaTime);
    }

    private void SpawnProjectile()
    {
        if (ProjectileManager.Instance.GameObjectPool.TryGet(projectileEvent.projectileEffectKey, out var model) == false) return;
        Model = model;
        Model.transform.SetParent(scaleTransform.transform);
        Model.transform.localPosition = Vector3.zero;

        switch (projectileEvent.projectileType)
        {
            case Projectiles.eProjectileType.Straight:
                projectile = Utilities.StaticeObjectPool.Pop<Projectile_Straight>();
                break;
            case Projectiles.eProjectileType.StraightFollow:
                projectile = Utilities.StaticeObjectPool.Pop<Projectile>();
                break;
        }

        projectile.Init(this, projectileEvent,caster.UnitState.team, caster, target.transform);
    }
}
