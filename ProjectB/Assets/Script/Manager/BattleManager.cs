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

    public int lv;
    public int exp;
    public int needExp;

    public SkillInfo[] mainSkillInfo = new SkillInfo[5];
    public Dictionary<eStat, double> runeAddStat = new Dictionary<eStat, double>();
    public Dictionary<eStat, double> passiveAddStat = new Dictionary<eStat, double>();

    public float getCurTime { get { return elaspedTime; } }

    public static BattleManager Instance
    {
        get { return Manager.Instance.GetManager<BattleManager>(); }
    }

    public void SetGame(float time)
    {
        stageTime = time;
        isGameEnd = false;
        isInit = true;
        lv = 1;
        exp = 0;
        var expRecord = TableManager.Instance.expTable.GetExpRecord(exp);
        needExp = expRecord.needExp;
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

    public void AddExp(int exp)
    {
        this.exp += exp;

        if (exp >= needExp)
            LevelUp();
    }

    public void LevelUp()
    {
        var expRecord = TableManager.Instance.expTable.GetExpRecord(exp);
        if (expRecord == null) return;

        lv += 1;
        exp -= needExp;
        needExp = expRecord.needExp;
        UILevelUpDlg dlg = UIManager.Instance.OpenWidget<UILevelUpDlg>();
        dlg.Open();
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
}
