using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public long curExp { get; private set; }
    public long needExp { get; private set; }
    public int curLv { get; private set; }
    private ExpRecord curExpRecord;

    private int preLv;
    public long killCount { get; private set; }

    public void Init()
    {
        curExp = 0;
        curLv = 1;
        killCount = 0;
    }

    public void AddExp(long exp)
    {
        curExp += exp;
        if (curExp >= needExp)
        {
            preLv = curLv;
            LevelUp();
        }
        PlayLogic.Instance.uiPlayLogic.UpdateExp();
    }

    public void LevelUp()
    {
        var expRecord = TableManager.Instance.expTable.GetExpRecord(curExp, out long remainExp);
        if (expRecord == null) return;

        curLv = expRecord.level;
        curExp = remainExp;
        needExp = expRecord.needExp;
        UILevelUpDlg dlg = UIManager.Instance.OpenWidget<UILevelUpDlg>();
        dlg.Open(curLv - preLv);
    }
    public void AddKillCount()
    {
        killCount += 1;
    }
}
