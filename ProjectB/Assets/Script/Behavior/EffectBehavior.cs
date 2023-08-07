using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehavior : MonoBehaviour
{
    private EffectManager manager;
    private GameObject particle;
    private float duration;
    private float elapased;
    private bool isInit;

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
        elapased = 0;
        isInit = true;
    }

    private void UnInit()
    {
        isInit = false;
        manager.GameObjectPool.Return(particle);
        manager.GameObjectPool.Return(gameObject);
        particle = null;
    }

    private void Update()
    {
        UpdateFrame(Time.deltaTime);
    }

    public void UpdateFrame(float deltaTime)
    {
        if (isInit == false)
            return;

        elapased += deltaTime;
        
        if (elapased > duration)
            UnInit();
    }

}
