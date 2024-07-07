using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : BaseManager
{
    private float stageTime;
    private float elaspedTime;
    private int waveNumber;
    private bool isInit;
    private bool isGameEnd;
    public bool isPause;
    private bool isWin;

    public Dictionary<eStat, double> runeAddStat = new Dictionary<eStat, double>();
    public Dictionary<eStat, double> passiveAddStat = new Dictionary<eStat, double>();

    public float getCurTime { get { return elaspedTime; } }

    public PlayerData playerData;
    private UIHpBarDlg uiHpBarDlg;
    public UnitBehavior player;
    public List<UnitBehavior> playerList = new List<UnitBehavior>();

    public Data.StageData stageData;
    public static BattleManager Instance
    {
        get { return Manager.Instance.GetManager<BattleManager>(); }
    }

    public override void Clear()
    {
        runeAddStat.Clear();
        passiveAddStat.Clear();
        playerList.Clear();
        playerData = null;
        uiHpBarDlg = null;

        base.Clear();
    }

    public void SetGame(int stageSeed)
    {
        if (!Data.DataManager.Instance.StageData.TryGet(stageSeed, out stageData)) return;
    }
    public void SetGame(float time)
    {
        stageTime = time;
        isGameEnd = false;
        isInit = true;

        playerData = new PlayerData();
        playerData.Init();

        uiHpBarDlg = UIManager.Instance.OpenWidget<UIHpBarDlg>(eWidgetType.Normal);
        uiHpBarDlg.Open();
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        if (isInit == false) return;
        if (isGameEnd) return;
        if (BattleManager.Instance.isPause) return;

        elaspedTime += DeltaTime;
        if (elaspedTime > stageTime) elaspedTime = stageTime;
    }

    public bool CheckEndGame()
    {
        isWin = false;
        if (UnitManager.Instance.Player != null && UnitManager.Instance.Player.UnitState.isDead) { isGameEnd = true; return true; }

        switch (stageData.Type)
        {
            case eStageType.Wave:
                {
                    var stageInfo = stageData.stageInfo as Data.StageData.WaveStage;
                    if (waveNumber >= stageInfo.waveCount) { isGameEnd = true; isWin = true; return true; }
                }
                break;
            case eStageType.Normal:
            default:
                {
                    var stageInfo = stageData.stageInfo as Data.StageData.NormalStage;
                    if (elaspedTime >= stageInfo.playTime) { isGameEnd = true; isWin = true;  return true; }
                }
                break;
        }
        return false;
    }

    public void CheckRuneAddStat(eStat statType, double value)
    {
        if (runeAddStat.ContainsKey(statType) == false)
            runeAddStat.Add(statType, 0);

        runeAddStat[statType] += value;
    }
    public void CheckPassiveAddStat(eStat statType, double value)
    {
        if (passiveAddStat.ContainsKey(statType) == false)
            passiveAddStat.Add(statType, 0);

        passiveAddStat[statType] += value;
    }
    public void SetHpBar(UnitBehavior unit)
    {
        uiHpBarDlg.RequestHpBar(unit);
    }
}
