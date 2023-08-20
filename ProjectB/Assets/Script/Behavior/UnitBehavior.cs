using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitState
{
    private Data.UnitData data;
    #region stat
    public long curHp;
    public long hp;
    public long atk;
    public long def;
    public float atkSpd;
    public float moveSpd;
    #endregion

    #region state
    public bool isDead;
    public bool isStun;
    public bool isSlow;
    public bool isMove;
    #endregion

    public void Init(Data.UnitData data)
    {
        this.data = data;
        isDead = false;
        isStun = false;
        isSlow = false;
        isMove = data.moveSpd > 0;

        SetStat();
    }

    public void SetStat()
    {
        hp = data.hp;
        curHp = hp;
        atk = data.atk;
        def = data.def;
        atkSpd = data.atkSpd;
        moveSpd = data.moveSpd;
    }
}

public class UnitBehavior : BaseBehavior, IEventHandler
{
    private Animator Animator;
    private AnimatorOverrideController animatorContorller;
    private int id;
    public int ID => id;
    private UnitBehavior target;

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

    private UnitManager manager;
    private UnitState unitState;
    public UnitState UnitState => unitState;

    #region AssetKey
    private const string SOUND_BEHAVIOR_ASSETKEY = "Assets/Data/GameResources/Prefab/Behavior/SoundBehavior.prefab";    
    #endregion

    public void Init(Data.UnitData data, int id)
    {
        manager = UnitManager.Instance;
        this.id = id;
        ElapsedTime = 0;

        timeScaleBehavior = Utilities.StaticeObjectPool.Pop<TimeScaleBehavior>();
        eventDispatcher = Utilities.StaticeObjectPool.Pop<EventDispatcher>();
        eventDispatcher.Init();

        Model = manager.GameObjectPool.Get(data.modelAssetRef);
        Model.transform.SetParent(scaleTransform.transform);
        animatorContorller = manager.ResourcePool.Load<AnimatorOverrideController>("Assets/GameResources/Animation/TestPlayer.overrideController");
        Animator = Model.GetComponent<Animator>();
        Animator.runtimeAnimatorController = animatorContorller;
        dicBoneTrf = Utilities.StaticeObjectPool.Pop<Dictionary<string, Transform>>();
        var boneList = Model.GetComponentsInChildren<Transform>();
        for (int i = 0; i < boneList.Length; i++)
        {
            dicBoneTrf.Add(boneList[i].name, boneList[i]);
        }

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
        Animator.speed = TimeScale;

        TestAction();
    }

    private void TestAction()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Action();
    }

    public void Action()
    {
        eventDispatcher.Clear();
        eventDispatcher.Add(manager.ResourcePool.Load<EventGraph>("Assets/GameResources/Animation/Action.asset"));
        Animator.CrossFadeInFixedTime("Action", 0.1f);
    }

    #region NodeEvent
    private void OnHandleSoundEvent(SoundEvent soundEvent)
    {
        if (!manager.GameObjectPool.TryGet(SOUND_BEHAVIOR_ASSETKEY, out var sound)) return;
        SoundBehavior soundBehavior = sound.GetComponent<SoundBehavior>();

        soundBehavior.Init(soundEvent);
    }
    private void OnHandleParticleEvent(ParticleEvent particleEvent)
    {
        var referenceBone = Model.transform;
        if(!string.IsNullOrEmpty(particleEvent.referenceBone))
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
        ProjectileManager.Instance.SpawnProjectile(projectileEvent, this, target);
    }
    #endregion

    public void ApplyDamage(float dmgPercent, Define.eDamageType dmgType = Define.eDamageType.Normal)
    {
        switch (dmgType)
        {
            case Define.eDamageType.Normal:
            default:
                unitState.curHp -= (long)(unitState.atk * dmgPercent);
                break;
            case Define.eDamageType.PerHp:
                unitState.curHp -= (long)(unitState.curHp * dmgPercent);
                break;
            case Define.eDamageType.PerMaxHp:
                unitState.curHp -= (long)(unitState.hp * dmgPercent);
                break;
        }

        if(unitState.curHp <= 0)
        {
            unitState.isDead = true;
        }
    }
}
