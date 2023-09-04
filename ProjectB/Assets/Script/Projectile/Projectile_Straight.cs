using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Straight : Projectile
{
    private Vector3 targetPos;

    Projectiles.Straight straight;
    public override void Init(ProjectileBehavior projectileBehavior, ProjectileEvent projectileEvent,eTeam team, Transform owner, Transform target)
    {
        base.Init(projectileBehavior, projectileEvent,team, owner, target);
        straight = projectileData as Projectiles.Straight;
        targetPos = target.position;
    }

    public override void Move()
    {
        moveDistance = DeltaTime * straight.Speed;
        projectileBehavior.transform.position += moveDistance * Vector3.Normalize(targetPos - projectileBehavior.transform.position);
        elapsedDistance += moveDistance;
    }

    public override bool CheckStop()
    {
        if (Vector3.Distance(targetPos, projectileBehavior.GetPos()) < 0.01f) return true;
        if (elapsedDistance > straight.MaxDistance) return true;

        return isHitEnd;
    }

    public override void CheckCollision()
    {
        int layer = team == eTeam.player ? LayerMask.GetMask("Monster") : LayerMask.GetMask("Player");
        var targets = Physics2D.OverlapCircleAll(transform.position, straight.Radius, layer);
        if (targets.Length <= 0) return;

        for (int i = 0; i < targets.Length; i++)
        {
            UnitBehavior hitUnit = targets[i].GetComponentInParent<UnitBehavior>();
            if (Vector3.Distance(hitUnit.GetPos(), projectileBehavior.GetPos()) > straight.Radius) continue;
            AddHitTarget(hitUnit);
        }
    }
}
