using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehavior : BaseBehavior
{
    private EffectManager manager;
    private GameObject particle;
    private float duration;
    public void Init(ParticleEvent particleEvent)
    {
        manager = EffectManager.Instance;

        if (!particleEvent.paricleObject.RuntimeKeyIsValid()) return;
        var assetKey = particleEvent.paricleObject.RuntimeKey;
        if (!manager.GameObjectPool.TryGet(assetKey, out var particleObject)) return;

        particle = particleObject;
        particle.transform.SetParent(transform);
        particle.transform.localPosition = Vector3.zero;
        duration = particleEvent.duration;
        elaspedTime = 0;
        isInit = true;
    }

    private void UnInit()
    {
        isInit = false;
        manager.GameObjectPool.Return(particle);
        manager.GameObjectPool.Return(gameObject);
        particle = null;
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isInit == false)
            return;
        base.UpdateFrame(deltaTime);
        
        if (elaspedTime > duration)
            UnInit();
    }

}
