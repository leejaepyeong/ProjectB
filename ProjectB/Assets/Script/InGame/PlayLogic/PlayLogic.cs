using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayLogic : BaseScene
{
    public static PlayLogic Instance;

    [FoldoutGroup("Setting")] public SpawnLogic spawnLogic;
    [FoldoutGroup("Setting")] public UIPlayLogic uiPlayLogic;
    [FoldoutGroup("Test")] public bool isTestMode;
    [ShowIf("@isTestMode"),FoldoutGroup("Test")] public int testStageSeed;


    protected ePlayLogicFsm curFsm;
    protected Coroutine coFsmSetting;
    protected float deltaTime;
    protected bool isSettingOn;
    protected int stageSeed;
    protected Data.StageData stageData;

    QueueCommand commands = new QueueCommand();
    IntroCommand_LoadDataFile loadDataFile = new IntroCommand_LoadDataFile();
    IntroCommand_LoadLocalData loadLocalData = new IntroCommand_LoadLocalData();

    public bool isTargetSkillOn;

    private Coroutine coMoveRangeObject;

    #region RangeObjectPath
    private const string RECT_RANGE_OBJECT = "Assets/Data/GameResources/Prefab/RangePrefab/Rect.prefab";
    private const string CIRCLE_RANGE_OBJECT = "Assets/Data/GameResources/Prefab/RangePrefab/Circle.prefab";
    private const string FANSHAPE_RANGE_OBJECT = "Assets/Data/GameResources/Prefab/RangePrefab/FanShape.prefab";
    #endregion
    public override void Init()
    {
        if (Instance == null)
            Instance = this;

        isSettingOn = false;

        commands.Add(loadDataFile);
        commands.Add(loadLocalData);
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isTestMode == false && (Manager.Instance.Loading.isActiveAndEnabled || Manager.Instance.Fade.isActiveAndEnabled)) return;
        if (BattleManager.Instance.isPause) return;

        this.deltaTime = deltaTime;
        commands.UpdateFrame(deltaTime);
        if (commands.CommandEnd() == false) return;

        if(curFsm == ePlayLogicFsm.none) ChangeFsm(ePlayLogicFsm.setting);
        if (isSettingOn == false) return;

        uiPlayLogic.UpdateFrame(deltaTime);
        UpdateFsm();
        if (BattleManager.Instance.CheckEndGame())
            ChangeFsm(ePlayLogicFsm.result);

        if (isTargetSkillOn && Input.GetMouseButtonDown(0))
            isTargetSkillOn = false;
    }

    protected virtual void UpdateFsm()
    {
        switch (curFsm)
        {
            case ePlayLogicFsm.ready:
                break;
            case ePlayLogicFsm.play:
                UpdatePlay();
                break;
            case ePlayLogicFsm.bossRound:
                UpdateBossRound();
                break;
            case ePlayLogicFsm.result:
                break;
            default:
                break;
        }
    }

    protected virtual void ChangeFsm(ePlayLogicFsm nextFsm)
    {
        if (curFsm == nextFsm) return;

        switch (nextFsm)
        {
            case ePlayLogicFsm.setting:
                EnterSetting();
                break;
            case ePlayLogicFsm.ready:
                EnterReady();
                break;
            case ePlayLogicFsm.play:
                EnterPlay();
                break;
            case ePlayLogicFsm.bossRound:
                EnterBossRound();
                break;
            case ePlayLogicFsm.result:
                EnterResult();
                break;
            default:
                break;
        }

        curFsm = nextFsm;
    }

    #region Setting
    protected virtual void EnterSetting()
    {
        if (isTestMode)
            stageSeed = testStageSeed;
        else
            stageSeed = SaveData_Local.Instance.lastPlayStageSeed;

        if (Data.DataManager.Instance.StageData.TryGet(stageSeed, out stageData) == false) return;

        BattleManager.Instance.SetGame(stageSeed);
        uiPlayLogic.Init();
        coFsmSetting = StartCoroutine(GameSettingCo());

        spawnLogic.Init();
    }

    protected IEnumerator GameSettingCo()
    {
        yield return new WaitForEndOfFrame();
        isSettingOn = true;
        ChangeFsm(ePlayLogicFsm.ready);
        coFsmSetting = null;
    }
    #endregion
    #region Ready
    protected virtual void EnterReady()
    {
        coFsmSetting = StartCoroutine(GameReadyCo());
    }

    protected IEnumerator GameReadyCo()
    {
        UnitManager.Instance.SpawnPlayer(1);

        yield return new WaitForEndOfFrame();
        ChangeFsm(ePlayLogicFsm.play);
        coFsmSetting = null;
    }
    #endregion
    #region Play
    protected virtual void EnterPlay()
    {

    }
    protected virtual void UpdatePlay()
    {
        spawnLogic.UpdateFrame(deltaTime);
    }

    #endregion
    #region Boss
    protected virtual void EnterBossRound()
    {

    }
    protected virtual void UpdateBossRound()
    {

    }
    #endregion
    #region Result
    protected virtual void EnterResult()
    {

    }
    #endregion

    #region Skill Area
    public void UseTargetSkill(SkillInfo skillInfo, UnityEngine.Events.UnityAction action = null)
    {
        if (isTargetSkillOn) return;
        isTargetSkillOn = true;

        List<RangeObject> rangeObjectList = new List<RangeObject>();
        var nodeList = skillInfo.skillRecord.skillNode.nodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i] is HitEvenet.HitEventNode hitEvent)
            {
                RangeObject rangeObj = null;
                switch (hitEvent.HitEvent.hitRange)
                {
                    case HitEvenet.eHitRange.Rect:
                        rangeObj = DrawRect(skillInfo, UnitManager.Instance.Player, hitEvent.HitEvent, action);
                        break;
                    case HitEvenet.eHitRange.Circle:
                        rangeObj = DrawCircle(skillInfo, UnitManager.Instance.Player, hitEvent.HitEvent, action);
                        break;
                    case HitEvenet.eHitRange.FanShape:
                        rangeObj = DrawFanShape(skillInfo, UnitManager.Instance.Player, hitEvent.HitEvent, action);
                        break;
                }
                rangeObjectList.Add(rangeObj);
            }
        }
        coMoveRangeObject = StartCoroutine(CoMoveRangeObject(rangeObjectList, skillInfo));
    }

    private RangeObject DrawRect(SkillInfo skillInfo, UnitBehavior caster, HitEvent hitEvent, UnityEngine.Events.UnityAction action= null)
    {
        if (BattleManager.Instance.GameObjectPool.TryGet(RECT_RANGE_OBJECT, out var rangeObj) == false) return null;
        var rectObj = rangeObj.GetComponent<RangeObject>();
        rectObj.Open(skillInfo, caster, hitEvent);

        return rectObj;
    }

    private RangeObject DrawCircle(SkillInfo skillInfo, UnitBehavior caster, HitEvent hitEvent, UnityEngine.Events.UnityAction action = null)
    {
        if (BattleManager.Instance.GameObjectPool.TryGet(CIRCLE_RANGE_OBJECT, out var rangeObj) == false) return null;
        var circleObj = rangeObj.GetComponent<RangeObject>();
        circleObj.Open(skillInfo, caster, hitEvent);

        return circleObj;
    }

    private RangeObject DrawFanShape(SkillInfo skillInfo, UnitBehavior caster, HitEvent hitEvent, UnityEngine.Events.UnityAction action = null)
    {
        if (BattleManager.Instance.GameObjectPool.TryGet(FANSHAPE_RANGE_OBJECT, out var rangeObj) == false) return null;
        var fanObj = rangeObj.GetComponent<RangeObject>();
        fanObj.Open(skillInfo, caster, hitEvent);

        return fanObj;
    }

    protected IEnumerator CoMoveRangeObject(RangeObject rangeObject)
    {
        rangeObject.transform.SetParent(EffectManager.Instance.transform);

        Vector3 position = Vector3.zero;
        while(isTargetSkillOn)
        {
            position = Manager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            rangeObject.transform.localPosition = new Vector3(position.x, position.y, 0);
            yield return new WaitForEndOfFrame();
        }

        rangeObject.OnClickAction(position);
        BattleManager.Instance.GameObjectPool.Return(rangeObject.gameObject);
        coMoveRangeObject = null;
    }
    protected IEnumerator CoMoveRangeObject(List<RangeObject> list, SkillInfo skillInfo)
    {
        Vector3 position = Vector3.zero;
        while (isTargetSkillOn)
        {
            position = Manager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].MoveMousePosition(new Vector3(position.x, position.y, 0));
            }
            yield return new WaitForEndOfFrame();
        }

        skillInfo.targetPos = position;
        skillInfo.UseSkill();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].OnClickAction(position);
            BattleManager.Instance.GameObjectPool.Return(list[i].gameObject);
        }

        coMoveRangeObject = null;
    }
    #endregion
}
