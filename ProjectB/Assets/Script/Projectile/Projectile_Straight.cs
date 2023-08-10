using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Straight : Projectile
{
    Projectiles.Straight straight;
    public override void Init(ProjectileBehavior projectileBehavior, ProjectileEvent projectileEvent, Transform owner, Transform target)
    {
        base.Init(projectileBehavior, projectileEvent, owner, target);
        straight = projectileData as Projectiles.Straight;
    }

    public override void Move()
    {
        moveDistance = DeltaTime * straight.Speed;
        transform.position += moveDistance * transform.forward;
        elapsedDistance += moveDistance;
    }

    public override bool CheckStop()
    {
        if (elapsedDistance > straight.MaxDistance)
            return true;

        return isHitEnd;
    }
}
