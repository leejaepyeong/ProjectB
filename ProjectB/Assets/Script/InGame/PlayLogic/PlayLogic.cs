using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ePlayLogicFsm
{
    none,
    setting,
    ready,
    play,
    bossRound,
    result,
}
public class PlayLogic : BaseScene
{
    public static PlayLogic Instance;

    [FoldoutGroup("Setting")] public SpawnLogic spawnLogic;
    [FoldoutGroup("Setting")] public UIPlayLogic uiPlayLogic;

    protected ePlayLogicFsm curFsm;
    protected Coroutine coFsmSetting;
    protected float deltaTime;
    protected bool isSettingOn;
    protected float playTime = 30f;

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
        BattleManager.Instance.SetGame(playTime);
        uiPlayLogic.Init();
        coFsmSetting = StartCoroutine(GameSettingCo());
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

        var nodeList = skillInfo.skillRecord.skillNode.nodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i] is HitEvenet.HitEventNode hitEvent)
            {
                switch (hitEvent.HitEvent.hitRange)
                {
                    case HitEvenet.eHitRange.Rect:
                        DrawRect(skillInfo, UnitManager.Instance.Player, hitEvent.HitEvent, action);
                        break;
                    case HitEvenet.eHitRange.Circle:
                        DrawCircle(skillInfo, UnitManager.Instance.Player, hitEvent.HitEvent, action);
                        break;
                    case HitEvenet.eHitRange.FanShape:
                        DrawFanShape(skillInfo, UnitManager.Instance.Player, hitEvent.HitEvent, action);
                        break;
                }
            }
        }
    }

    private void DrawRect(SkillInfo skillInfo, UnitBehavior caster, HitEvent hitEvent, UnityEngine.Events.UnityAction action= null)
    {
        if (BattleManager.Instance.GameObjectPool.TryGet(RECT_RANGE_OBJECT, out var rangeObj) == false) return;
        var rectObj = rangeObj.GetComponent<RangeObject>();
        rectObj.Open(skillInfo, caster, hitEvent);

        coMoveRangeObject = StartCoroutine(CoMoveRangeObject(rectObj));
    }

    private void DrawCircle(SkillInfo skillInfo, UnitBehavior caster, HitEvent hitEvent, UnityEngine.Events.UnityAction action = null)
    {
        if (BattleManager.Instance.GameObjectPool.TryGet(CIRCLE_RANGE_OBJECT, out var rangeObj) == false) return;
        var circleObj = rangeObj.GetComponent<RangeObject>();
        circleObj.Open(skillInfo, caster, hitEvent);

        coMoveRangeObject = StartCoroutine(CoMoveRangeObject(circleObj));
    }

    private void DrawFanShape(SkillInfo skillInfo, UnitBehavior caster, HitEvent hitEvent, UnityEngine.Events.UnityAction action = null)
    {
        if (BattleManager.Instance.GameObjectPool.TryGet(FANSHAPE_RANGE_OBJECT, out var rangeObj) == false) return;
        var fanObj = rangeObj.GetComponent<RangeObject>();
        fanObj.Open(skillInfo, caster, hitEvent);

        coMoveRangeObject = StartCoroutine(CoMoveRangeObject(fanObj));
    }

    protected IEnumerator CoMoveRangeObject(RangeObject rangeObject)
    {
        rangeObject.transform.SetParent(EffectManager.Instance.transform);

        Vector3 position = Vector3.zero;
        while(isTargetSkillOn)
        {
            position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rangeObject.transform.localPosition = new Vector3(position.x, position.y, 0);
            yield return new WaitForEndOfFrame();
        }

        rangeObject.OnClickAction(position);
        BattleManager.Instance.GameObjectPool.Return(rangeObject.gameObject);
        coMoveRangeObject = null;
    }
    #endregion
}
