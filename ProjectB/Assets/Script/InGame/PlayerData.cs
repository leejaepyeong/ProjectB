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

    public List<InvenItemInfo> invenItemList = new List<InvenItemInfo>();
    private Dictionary<int, InvenItemInfo> dicItem = new Dictionary<int, InvenItemInfo>();
    private int itemId;

    public void Init()
    {
        curExp = 0;
        curLv = 1;
        killCount = 0;
        itemId = 0;
        invenItemList.Clear();
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

    #region Item
    public void AddISkilltem(int skillIdx)
    {
        itemId += 1;
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, skillIdx, eItemType.Skill);
        invenItemList.Add(itemInfo);
        dicItem.Add(itemId, itemInfo);
    }

    public void AddRuneItem(int runeIdx)
    {
        itemId += 1;
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, runeIdx, eItemType.Rune);
        invenItemList.Add(itemInfo);
        dicItem.Add(itemId, itemInfo);
    }
    public void AddUseItem(int useIndex)
    {
        itemId += 1;
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, useIndex, eItemType.Use);
        invenItemList.Add(itemInfo);
        dicItem.Add(itemId, itemInfo);
    }

    public void RemoveItem(int itemId)
    {
        if (dicItem.TryGetValue(itemId, out var item) == false) return;

        invenItemList.Remove(item);
        item = null;
    }
    #endregion
}
