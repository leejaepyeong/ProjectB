using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehavior : BaseBehavior
{
    private EffectManager manager;
    private float duration;
    public void Init(ParticleEvent particleEvent)
    {
        manager = EffectManager.Instance;

        if (!particleEvent.paricleObject.RuntimeKeyIsValid()) return;
        var assetKey = particleEvent.paricleObject.RuntimeKey;
        if (!manager.GameObjectPool.TryGet(assetKey, out var particleObject)) return;

        Model = particleObject;
        Model.transform.SetParent(transform);
        Model.transform.localPosition = Vector3.zero;
        duration = particleEvent.duration;
        elaspedTime = 0;
        isInit = true;
    }

    private void UnInit()
    {
        isInit = false;
        manager.GameObjectPool.Return(Model);
        manager.GameObjectPool.Return(gameObject);
        Model = null;
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isInit == false)
            return;
        base.UpdateFrame(deltaTime);
        elaspedTime += deltaTime;
        if (elaspedTime > duration)
            UnInit();
    }

}
