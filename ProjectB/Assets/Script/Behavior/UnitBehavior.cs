using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffState
{

}

public class UnitState
{
    private Data.UnitData data;
    public eTeam team; 
    #region stat
    public long hp;
    public long mp;
    public long atk;
    public long def;
    public float acc;
    public float atkSpd;
    public float moveSpd;
    public float atkRange;
    public float criRate;
    public float criDmg;
    public Dictionary<eStat, long> statDic = new Dictionary<eStat, long>();
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

        SetStat();
        atkInfo = new SkillInfo(data.atkInfo.GetCopyRecord());
    }

    public void SetStat()
    {
        hp = data.hp;
        mp = data.mp;
        atk = data.atk;
        def = data.def;
        acc = data.acc;
        atkSpd = data.atkSpd;
        moveSpd = data.moveSpd;
        atkRange = data.atkRange;
        criRate = data.criRate;
        criDmg = data.criDmg;
    }
    public void SetStat(eStat statType, bool isTestScene)
    {
        switch (statType)
        {
            case eStat.hp: hp += isTestScene ? Manager.Instance.playerData.hp : SaveData_Local.Instance.userStat.hp; break;
            case eStat.mp: mp += isTestScene ? Manager.Instance.playerData.mp : SaveData_Local.Instance.userStat.mp; break;
            case eStat.atk:atk += isTestScene ? Manager.Instance.playerData.atk : SaveData_Local.Instance.userStat.atk; break;
            case eStat.def: def += isTestScene ? Manager.Instance.playerData.def : SaveData_Local.Instance.userStat.def; break;
            case eStat.acc: acc += isTestScene ? Manager.Instance.playerData.acc : SaveData_Local.Instance.userStat.acc; break;
            case eStat.moveSpd: moveSpd += isTestScene ? Manager.Instance.playerData.moveSpd : SaveData_Local.Instance.userStat.moveSpd; break;
            case eStat.atkSpd: atkSpd += isTestScene ? Manager.Instance.playerData.atkSpd : SaveData_Local.Instance.userStat.atkSpd; break;
            case eStat.atkRange: atkRange += isTestScene ? Manager.Instance.playerData.atkRange : SaveData_Local.Instance.userStat.atkRange; break;
            case eStat.criRate: criRate += isTestScene ? Manager.Instance.playerData.criRate : SaveData_Local.Instance.userStat.criRate; break;
            case eStat.criDmg: criDmg += isTestScene ? Manager.Instance.playerData.criDmg : SaveData_Local.Instance.userStat.criDmg; break;
        }
    }

    public void AddStat(eStat statType, double value)
    {
        switch (statType)
        {
            case eStat.hp: hp += (long)value; break;
            case eStat.mp: mp += (long)value; break;
            case eStat.atk: atk += (long)value; break;
            case eStat.def: def += (long)value; break;
            case eStat.acc: acc += (float)value; break;
            case eStat.moveSpd: moveSpd += (float)value; break;
            case eStat.atkSpd: atkSpd += (float)value; break;
            case eStat.atkRange: atkRange += (float)value; break;
            case eStat.criRate: criRate += (float)value; break;
            case eStat.criDmg: criDmg += (float)value; break;
        }
    }

    public double GetStat(eStat statType)
    {
        double value = 0;

        switch (statType)
        {
            case eStat.hp: value = hp; break;
            case eStat.mp: value = mp; break;
            case eStat.atk: value = atk; break;
            case eStat.def: value = def; break;
            case eStat.acc: value = acc; break;
            case eStat.moveSpd: value = moveSpd; break;
            case eStat.atkSpd: value = atkSpd; break;
            case eStat.atkRange: value = atkRange; break;
            case eStat.criRate: value = criRate; break;
            case eStat.criDmg: value = criDmg; break;
        }

        return 0;
    }
}

public class UnitBehavior : BaseBehavior, IEventHandler
{
    private Animator Animator;
    private AnimatorOverrideController animatorContorller;
    private int id;
    public int ID => id;
    private List<UnitBehavior> targets = new List<UnitBehavior>();

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

    public bool isUseSkill;
    public SkillInfo skillInfo;

    //Monster
    public void Init(Data.UnitData data, int id)
    {
        manager = UnitManager.Instance;
        unitData = data;
        this.id = id;
        ElapsedTime = 0;
        isUseSkill = false;
        skillInfo = null;

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
        Animator.CrossFadeInFixedTime(eventGraph.name, 0f);
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
        if(isUseSkill && skillInfo == null)
        {
            isUseSkill = false;
            return;
        }

        for (int i = 0; i < targets.Count; i++)
        {
            ProjectileManager.Instance.SpawnProjectile(isUseSkill ? skillInfo : unitState.atkInfo, projectileEvent, this, targets[i]);
        }
    }
    #endregion

    public void SetTargets(List<UnitBehavior> targets)
    {
        this.targets.Clear();
        for (int i = 0; i < targets.Count; i++)
        {
            this.targets.Add(targets[i]);
        }
    }
}
