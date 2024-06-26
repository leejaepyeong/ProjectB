using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : BaseManager
{
    private List<HitBehavior> hitList = new();

    private const string HIT_BEHAVIOR_ASSETKEY = "Assets/Data/GameResources/Prefab/Behavior/HitBehavior.prefab";

    public static HitManager Instance
    {
        get { return Manager.Instance.GetManager<HitManager>(); }
    }

    public override void Init()
    {
        base.Init();
        hitList.Clear();
    }

    public override void Clear()
    {
        hitList.Clear();
        base.Clear();
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (BattleManager.Instance.isPause) return;
        base.UpdateFrame(deltaTime);
        for (int i = 0; i < hitList.Count; i++)
        {
            if (hitList[i].isActiveAndEnabled == false) continue;
            hitList[i].UpdateFrame(DeltaTime);
        }
    }

    public HitBehavior SpawnHit(HitEvent hitEvent, UnitBehavior caster = null, UnitBehavior target = null)
    {
        if (!GameObjectPool.TryGet(HIT_BEHAVIOR_ASSETKEY, out var hitObj)) return null;
        HitBehavior hit = hitObj.GetComponent<HitBehavior>();

        hit.transform.SetParent(transform);
        hit.Init(hitEvent, caster, target);
        hitList.Add(hit);

        return hit;
    }

    public void RemoveHit(HitBehavior hit)
    {
        hitList.Remove(hit);
    }

    public void RemoveAllHit()
    {
        while (hitList.Count > 0)
        {
            hitList[0].Close();
        }
    }
}
