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

        raycastHitCnt = Physics.SphereCastNonAlloc(transform.position, straight.Radius, Vector3.up, raycastHits, 0, Define.UNIT_LAYER);
        if(raycastHitCnt > 0)
        {
            switch (projectileEvent.projectileHit)
            {
                case Projectiles.eProjectileHit.Normal:
                    {
                        UnitBehavior unit = raycastHits[0].collider.GetComponent<UnitBehavior>();
                        targetList.Add(unit.ID, unit);
                        unit.HitParticleEvent(straight.HitEvent, raycastHits[0].point);
                        return true;
                    }
                case Projectiles.eProjectileHit.Penetration:
                    for (int i = 0; i < raycastHitCnt; i++)
                    {
                        UnitBehavior unit = raycastHits[i].collider.GetComponent<UnitBehavior>();
                        if (targetList.ContainsKey(unit.ID))
                            continue;
                        targetList.Add(unit.ID, unit);
                        unit.HitParticleEvent(straight.HitEvent, raycastHits[i].point);
                    }
                    break;
            }
        }

        return false;
    }
}
