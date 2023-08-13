using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float elaspedTime;
    private ProjectileEvent projectileEvent;
    private Projectile projectile;

    private UnitBehavior caster;
    private UnitBehavior target;

    protected bool isInit;
    public bool IsInit => isInit;

    public void Init(ProjectileEvent projectileEvent, UnitBehavior caster, UnitBehavior target)
    {
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

    public void UpdateFrame(float deltaTime)
    {
        elaspedTime += deltaTime;
        projectile.UpdateFrame(deltaTime);
    }

    private void SpawnProjectile()
    {
        switch (projectileEvent.projectileType)
        {
            case Projectiles.eProjectileType.Straight:
                projectile = Utilities.StaticeObjectPool.Pop<Projectile_Straight>();
                break;
            case Projectiles.eProjectileType.StraightFollow:
                projectile = Utilities.StaticeObjectPool.Pop<Projectile>();
                break;
            case Projectiles.eProjectileType.Parabolic:
                projectile = Utilities.StaticeObjectPool.Pop<Projectile>();
                break;
        }

        projectile.Init(this, projectileEvent, caster.transform, target.transform);
    }

    #region Collision
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Unit")
        {
            UnitBehavior hitUnit = other.GetComponent<UnitBehavior>();
            projectile.AddHitTarget(hitUnit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Unit")
        {
            UnitBehavior hitUnit = other.GetComponent<UnitBehavior>();
            projectile.RemoveHitTarget(hitUnit);
        }
    }
    #endregion
}
