using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : BaseManager
{

    private const string EFFECT_BEHAVIOR_ASSETKEY = "Assets/Data/GameResources/Prefab/Behavior/EffectBehavior.prefab";
    public static EffectManager Instance
    {
        get { return Manager.Instance.GetManager<EffectManager>(); }
    }

    private List<EffectBehavior> effectList = new();

    public override void Init()
    {
        base.Init();
        effectList.Clear();
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
    }

    public void SpawnEffect(ParticleEvent particleEvent, Transform spawnTrf)
    {
        if (!GameObjectPool.TryGet(EFFECT_BEHAVIOR_ASSETKEY, out var effect)) return;
        EffectBehavior effectBehavior = effect.GetComponent<EffectBehavior>();

        effectBehavior.transform.SetParent(spawnTrf);
        effectBehavior.transform.position = spawnTrf.position + particleEvent.localPosition;
        effectBehavior.transform.localEulerAngles = particleEvent.localEular;
        effectBehavior.transform.localScale = particleEvent.localScale;
        effectBehavior.Init(particleEvent);

        effectList.Add(effectBehavior);
    }
    public void SpawnEffect(ParticleEvent particleEvent, Vector3 hitPos)
    {
        if (!GameObjectPool.TryGet(EFFECT_BEHAVIOR_ASSETKEY, out var effect)) return;
        EffectBehavior effectBehavior = effect.GetComponent<EffectBehavior>();

        effectBehavior.transform.SetParent(null);
        effectBehavior.transform.position = hitPos;
        effectBehavior.transform.localEulerAngles = particleEvent.localEular;
        effectBehavior.transform.localScale = particleEvent.localScale;
        effectBehavior.Init(particleEvent);

        effectList.Add(effectBehavior);
    }


    public void RemoveEffect(EffectBehavior effect)
    {
        effectList.Remove(effect);
    }
}
