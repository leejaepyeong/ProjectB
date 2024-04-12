using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : BaseManager
{
    private float stageTime;
    private float elaspedTime;
    private bool isInit;
    private bool isGameEnd;
    public bool isPause;

    public SkillInfo[] mainSkillInfo = new SkillInfo[5];
    public Dictionary<eStat, double> runeAddStat = new Dictionary<eStat, double>();
    public Dictionary<eStat, double> passiveAddStat = new Dictionary<eStat, double>();

    public float getCurTime { get { return elaspedTime; } }

    public PlayerData playerData;
    private UIHpBarDlg uiHpBarDlg;

    public static BattleManager Instance
    {
        get { return Manager.Instance.GetManager<BattleManager>(); }
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

        elaspedTime += DeltaTime;
        if (elaspedTime > stageTime) elaspedTime = stageTime;

    }

    public bool CheckEndGame()
    {
        if (elaspedTime >= stageTime) { isGameEnd = true; return true; }
        if (UnitManager.Instance.Player != null && UnitManager.Instance.Player.UnitState.isDead) { isGameEnd = true; return true; }

        return false;
    }

    public void SetMainSkillSlot(int index, SkillInfo skillInfo)
    {
        mainSkillInfo[index] = skillInfo;
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
