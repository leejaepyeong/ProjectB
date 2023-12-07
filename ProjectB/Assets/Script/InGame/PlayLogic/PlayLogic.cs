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

    QueueCommand commands = new QueueCommand();
    IntroCommand_LoadDataFile loadDataFile = new IntroCommand_LoadDataFile();
    IntroCommand_LoadLocalData loadLocalData = new IntroCommand_LoadLocalData();

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
        BattleManager.Instance.SetGame(30f);
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
}
