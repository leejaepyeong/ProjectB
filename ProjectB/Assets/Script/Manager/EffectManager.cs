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
    public override void Clear()
    {
        effectList.Clear();
        base.Clear();
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (BattleManager.Instance.isPause) return;
        base.UpdateFrame(deltaTime);
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].UpdateFrame(deltaTime);
        }
    }

    public void SpawnEffect(ParticleEvent particleEvent, Transform spawnTrf)
    {
        if (!GameObjectPool.TryGet(EFFECT_BEHAVIOR_ASSETKEY, out var effect)) return;
        EffectBehavior effectBehavior = effect.GetComponent<EffectBehavior>();
        effectBehavior.transform.SetParent(transform);

        switch (particleEvent.effectSpawn)
        {
            case ParticleEvent.eEffectSpawn.Custom:
                effectBehavior.transform.position = spawnTrf.position + particleEvent.position;
                effectBehavior.transform.localEulerAngles = particleEvent.eular;
                break;
            case ParticleEvent.eEffectSpawn.Center:
                effectBehavior.transform.position = Vector3.zero;
                effectBehavior.transform.localEulerAngles = Vector3.zero;
                break;
        }
        effectBehavior.transform.localScale = particleEvent.scale;
        effectBehavior.Init(particleEvent);

        effectList.Add(effectBehavior);
    }
    public void SpawnEffect(ParticleEvent particleEvent, Vector3 hitPos, UnitBehavior hitUnit = null)
    {
        if (!GameObjectPool.TryGet(EFFECT_BEHAVIOR_ASSETKEY, out var effect)) return;
        EffectBehavior effectBehavior = effect.GetComponent<EffectBehavior>();
        effectBehavior.transform.SetParent(transform);

        switch (particleEvent.effectSpawn)
        {
            case ParticleEvent.eEffectSpawn.Center:
                effectBehavior.transform.position = Vector3.zero;
                effectBehavior.transform.localEulerAngles = Vector3.zero;
                break;
            case ParticleEvent.eEffectSpawn.HitPosition:
                effectBehavior.transform.position = hitPos;
                effectBehavior.transform.localEulerAngles = particleEvent.eular;
                break;
        }
        effectBehavior.transform.localScale = particleEvent.scale;
        effectBehavior.Init(particleEvent);

        effectList.Add(effectBehavior);
    }


    public void RemoveEffect(EffectBehavior effect)
    {
        effectList.Remove(effect);
    }
}
