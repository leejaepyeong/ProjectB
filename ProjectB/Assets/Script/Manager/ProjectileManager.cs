using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : BaseManager
{
    private List<ProjectileBehavior> projectileList = new();

    private const string PROJECTILE_BEHAVIOR_ASSETKEY = "Assets/Data/GameResources/Prefab/Behavior/ProjectileBehavior.prefab";

    public static ProjectileManager Instance
    {
        get { return Manager.Instance.GetManager<ProjectileManager>(); }
    }

    public override void Init()
    {
        base.Init();
        projectileList.Clear();
    }

    public void DeInit()
    {
        GameObjectPool = null;
        ResourcePool = null;
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        for (int i = 0; i < projectileList.Count; i++)
        {
            if (projectileList[i].isActiveAndEnabled == false) continue;
            projectileList[i].UpdateFrame(DeltaTime);
        }
    }

    public ProjectileBehavior SpawnProjectile(ProjectileEvent projectileEvent, UnitBehavior caster = null, UnitBehavior target = null)
    {
        if (!GameObjectPool.TryGet(PROJECTILE_BEHAVIOR_ASSETKEY, out var projectileObj)) return null;
        ProjectileBehavior projectile = projectileObj.GetComponent<ProjectileBehavior>();

        projectile.transform.SetParent(transform);
        projectile.transform.position = caster.GetPos();
        projectile.Init(projectileEvent, caster, target);
        projectileList.Add(projectile);

        return projectile;
    }

    public void RemoveProjectile(ProjectileBehavior projectile)
    {
        projectileList.Remove(projectile);
        GameObjectPool.Return(projectile.Model);
        GameObjectPool.Return(projectile.gameObject);
    }

    public void RemoveAllProjectile()
    {
        while(projectileList.Count > 0)
        {
            RemoveProjectile(projectileList[0]);
        }
    }
}
