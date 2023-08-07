using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float elasped;
    private float duration;
    private ProjectileEvent projectileEvent;
    private Projectile projectile;

    private UnitBehavior caster;
    private UnitBehavior target;

    public void Init(ProjectileEvent projectileEvent, UnitBehavior caster, UnitBehavior target)
    {
        this.projectileEvent = projectileEvent;
        this.caster = caster;
        this.target = target;
        elasped = 0;
        SpawnProjectile();
    }

    public void UpdateFrame(float deltaTime)
    {
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
}
