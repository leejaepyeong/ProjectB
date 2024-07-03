using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehavior : BaseBehavior, IEventHandler
{
    private int id;
    public int ID => id;

    private EventDispatcher eventDispatcher;
    private Dictionary<string, Transform> dicBoneTrf;

    #region Time
    private TimeScaleBehavior timeScaleBehavior;
    public float DeltaTime { get; private set; }
    public float TimeScale { get; private set; }
    public float ElapsedTime { get; private set; }
    public float InternalTimeScale
    {
        get { return timeScaleBehavior.TimeScale; }
    }
    #endregion

    private UnitBehavior caster;
    private EffectManager manager;

    #region AssetKey
    private const string SOUND_BEHAVIOR_ASSETKEY = "Assets/Data/GameResources/Prefab/Behavior/SoundBehavior.prefab";
    #endregion

    //Monster
    public void Init(Data.UnitData data, int id)
    {
        manager = EffectManager.Instance;
        this.id = id;
        ElapsedTime = 0;

        timeScaleBehavior = Utilities.StaticeObjectPool.Pop<TimeScaleBehavior>();
        eventDispatcher = Utilities.StaticeObjectPool.Pop<EventDispatcher>();
        eventDispatcher.Init(caster);

        isInit = true;
    }

    public void Close()
    {
        isInit = false;
        manager.GameObjectPool.Return(Model);
    }

    public void OnHandleEvent(EventNodeData eventData)
    {
        switch (eventData)
        {
            case SoundEvent soundEvent:
                OnHandleSoundEvent(soundEvent);
                break;
            case ParticleEvent particleEvent:
                OnHandleParticleEvent(particleEvent);
                break;
            case TimeScaleEvent timeScaleEvent:
                OnHandleTimeScaleEvent(timeScaleEvent);
                break;
            case ProjectileEvent projectileEvent:
                OnHandleProjectileEvent(projectileEvent);
                break;
            case HitEvent hitEvent:
                OnHandleHitEvent(hitEvent);
                break;
            default:
                break;
        }
    }

    public void UpdateFrame(float deltaTime, float timeScale)
    {
        if (isInit == false)
            return;

        timeScaleBehavior.UpdateFrame(deltaTime);

        DeltaTime = deltaTime * InternalTimeScale;
        TimeScale = timeScale * InternalTimeScale;

        ElapsedTime += DeltaTime;

        eventDispatcher.UpdateFrame(DeltaTime, this);
    }

    #region NodeEvent
    public void Action(EventGraph eventGraph)
    {
        eventDispatcher.Clear();
        eventDispatcher.Add(eventGraph);
    }

    private void OnHandleSoundEvent(SoundEvent soundEvent)
    {
        if (!manager.GameObjectPool.TryGet(SOUND_BEHAVIOR_ASSETKEY, out var sound)) return;
        SoundBehavior soundBehavior = sound.GetComponent<SoundBehavior>();

        soundBehavior.Init(soundEvent);
    }
    private void OnHandleParticleEvent(ParticleEvent particleEvent)
    {
        var referenceBone = Model.transform;
        if (!string.IsNullOrEmpty(particleEvent.referenceBone))
        {
            if (dicBoneTrf.TryGetValue(particleEvent.referenceBone, out var bone))
                referenceBone = bone;
        }
        EffectManager.Instance.SpawnEffect(particleEvent, referenceBone);
    }

    public void HitParticleEvent(ParticleEvent particleEvent, Vector3 spawnPos)
    {

    }

    private void OnHandleTimeScaleEvent(TimeScaleEvent timeScaleEvent)
    {
        timeScaleBehavior.Init(timeScaleEvent.animationCurve);
    }

    private void OnHandleProjectileEvent(ProjectileEvent projectileEvent)
    {
        var targetList = Manager.Instance.skillManager.GetTargetList(caster, projectileEvent.SkillInfo.skillRecord);

        for (int i = 0; i < targetList.Count; i++)
        {
            ProjectileManager.Instance.SpawnProjectile(projectileEvent.SkillInfo, projectileEvent, caster, targetList[i]);
        }
    }
    private void OnHandleHitEvent(HitEvent hitEvent)
    {
        HitManager.Instance.SpawnHit(hitEvent, caster);
    }
    #endregion
}
