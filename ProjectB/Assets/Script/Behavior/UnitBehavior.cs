using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffState
{
    public List<BuffBase> buffList = new List<BuffBase>();

    public void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            buffList[i].UpdateFrame(deltaTime);
            if (buffList[i].CheckEndBuff())
                buffList.Remove(buffList[i]);
        }
    }
}

public class UnitState
{
    private Data.UnitData data;
    public eTeam team; 
    #region stat
    public Dictionary<eStat, double> originStatValue = new Dictionary<eStat, double>();
    public Dictionary<eStat, double> statDic = new Dictionary<eStat, double>();
    #endregion
    public SkillInfo atkInfo;

    #region state
    public bool isDead;
    public bool isStun;
    public bool isSlow;
    public bool isMoveAble { get { return isStun == false; } }
    #endregion

    #region Buff
    public BuffState buffState;
    #endregion

    public void Init(Data.UnitData data)
    {
        this.data = data;
        team = data.Type == eUnitType.Player ? eTeam.player : eTeam.monster;
        isDead = false;
        isStun = false;
        isSlow = false;

        if(originStatValue.ContainsKey(eStat.hp) == false)
        {
            for (int i = 0; i < (int)eStat.END; i++)
            {
                originStatValue.Add((eStat)i, 0);
                statDic.Add((eStat)i, 0);
            }
        }

        SetStat();
        atkInfo = new SkillInfo(data.atkInfo);
    }

    public void SetStat()
    {
        originStatValue[eStat.hp] = data.hp;
        originStatValue[eStat.mp] = data.mp;
        originStatValue[eStat.atk] = data.atk;
        originStatValue[eStat.def] = data.def;
        originStatValue[eStat.acc] = data.acc;
        originStatValue[eStat.moveSpd] = data.moveSpd;
        originStatValue[eStat.atkSpd] = data.atkSpd;
        originStatValue[eStat.atkRange] = data.atkRange;
        originStatValue[eStat.criDmg] = data.criDmg;
        originStatValue[eStat.criRate] = data.criRate;
    }
    public void SetPlayerStat(eStat statType, bool isTestScene)
    {
        if (originStatValue.ContainsKey(statType) == false) originStatValue.Add(statType, 0);
        switch (statType)
        {
            case eStat.hp: originStatValue[eStat.hp] += isTestScene ? Manager.Instance.playerData.hp : SaveData_Local.Instance.userStat.hp; break;
            case eStat.mp: originStatValue[eStat.mp] += isTestScene ? Manager.Instance.playerData.mp : SaveData_Local.Instance.userStat.mp; break;
            case eStat.atk: originStatValue[eStat.atk] += isTestScene ? Manager.Instance.playerData.atk : SaveData_Local.Instance.userStat.atk; break;
            case eStat.def: originStatValue[eStat.def] += isTestScene ? Manager.Instance.playerData.def : SaveData_Local.Instance.userStat.def; break;
            case eStat.acc: originStatValue[eStat.acc] += isTestScene ? Manager.Instance.playerData.acc : SaveData_Local.Instance.userStat.acc; break;
            case eStat.moveSpd: originStatValue[eStat.moveSpd] += isTestScene ? Manager.Instance.playerData.moveSpd : SaveData_Local.Instance.userStat.moveSpd; break;
            case eStat.atkSpd: originStatValue[eStat.atkSpd] += isTestScene ? Manager.Instance.playerData.atkSpd : SaveData_Local.Instance.userStat.atkSpd; break;
            case eStat.atkRange: originStatValue[eStat.atkRange] += isTestScene ? Manager.Instance.playerData.atkRange : SaveData_Local.Instance.userStat.atkRange; break;
            case eStat.criRate: originStatValue[eStat.criRate] += isTestScene ? Manager.Instance.playerData.criRate : SaveData_Local.Instance.userStat.criRate; break;
            case eStat.criDmg: originStatValue[eStat.criDmg] += isTestScene ? Manager.Instance.playerData.criDmg : SaveData_Local.Instance.userStat.criDmg; break;
        }
    }

    public double GetStat(eStat statType)
    {
        double value = originStatValue[statType];

        return value;
    }
}

public class UnitBehavior : BaseBehavior, IEventHandler
{
    private Animator Animator;
    private AnimatorOverrideController animatorContorller;
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

    private UnitManager manager;
    private Data.UnitData unitData;
    public Data.UnitData UnitData => unitData;
    private UnitState unitState = new UnitState();
    private Unit_Base unitBase;
    public UnitState UnitState => unitState;
    public Unit_Base UnitBase => unitBase;

    #region AssetKey
    private const string SOUND_BEHAVIOR_ASSETKEY = "Assets/Data/GameResources/Prefab/Behavior/SoundBehavior.prefab";
    #endregion

    //Monster
    public void Init(Data.UnitData data, int id)
    {
        manager = UnitManager.Instance;
        unitData = data;
        this.id = id;
        ElapsedTime = 0;

        timeScaleBehavior = Utilities.StaticeObjectPool.Pop<TimeScaleBehavior>();
        eventDispatcher = Utilities.StaticeObjectPool.Pop<EventDispatcher>();
        eventDispatcher.Init(this);

        if (UnitManager.Instance.GameObjectPool.TryGet(data.modelAssetRef, out var model) == false) return;
        Model = model;
        Model.transform.SetParent(scaleTransform.transform);
        Model.transform.localPosition = Vector3.zero;
        Model.transform.localScale = Vector3.one;
        Animator = Model.GetComponent<Animator>();
        if(string.IsNullOrEmpty(data.animatorAssetRef) == false)
        {
            animatorContorller = manager.ResourcePool.Load<AnimatorOverrideController>(data.animatorAssetRef);
            Animator = Model.GetComponent<Animator>();
            Animator.runtimeAnimatorController = animatorContorller;
        }
        dicBoneTrf = Utilities.StaticeObjectPool.Pop<Dictionary<string, Transform>>();
        var boneList = Model.GetComponentsInChildren<Transform>();
        for (int i = 0; i < boneList.Length; i++)
        {
            dicBoneTrf.Add(boneList[i].name, boneList[i]);
        }

        unitState.Init(data);
        if(data.Type == eUnitType.Player)
            unitBase = new Unit_Player();
        else
            unitBase = new Unit_Monster();
        unitBase.Init(this);
        int layer = data.Type == eUnitType.Player ? LayerMask.NameToLayer("Player") : LayerMask.NameToLayer("Monster");
        gameObject.layer = layer;
        col.gameObject.layer = layer;

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
        unitBase.UpdateFrame(DeltaTime);
        Animator.speed = TimeScale;
    }

    #region NodeEvent
    public void Action(EventGraph eventGraph)
    {
        eventDispatcher.Clear();
        eventDispatcher.Add(eventGraph);
        Animator.CrossFadeInFixedTime(eventGraph.animationName, 0f);
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
        var targetList = Manager.Instance.skillManager.GetTargetList(this, projectileEvent.SkillInfo.skillRecord);

        for (int i = 0; i < targetList.Count; i++)
        {
            ProjectileManager.Instance.SpawnProjectile(projectileEvent.SkillInfo, projectileEvent, this, targetList[i]);
        }
    }
    private void OnHandleHitEvent(HitEvent hitEvent)
    {
        HitManager.Instance.SpawnHit(hitEvent, this);
    }
    #endregion
}
