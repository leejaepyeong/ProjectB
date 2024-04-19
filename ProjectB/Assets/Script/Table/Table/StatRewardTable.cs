using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatRewardRecord : RecordBase
{
    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public string iconPath;
    public eLevelUpReward rewardType;
    public eStat statType;
    public eUseType useType;
    public eAddType addType;
    public float addValue;
    public int maxStack;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "GroupId");
        nameIdx = FileUtil.Get<int>(_data, "Name");
        destIdx = FileUtil.Get<int>(_data, "Desc");
        iconPath = FileUtil.Get<string>(_data, "IconPath");
        rewardType = FileUtil.Get<eLevelUpReward>(_data, "RewardType");
        statType = FileUtil.Get<eStat>(_data, "StatType");
        useType = FileUtil.Get<eUseType>(_data, "UseType");
        addType = FileUtil.Get<eAddType>(_data, "AddType");
        addValue = FileUtil.Get<float>(_data, "AddValue");
        maxStack = FileUtil.Get<int>(_data, "MaxStack");
    }
    public string getName => TableManager.Instance.stringTable.GetText(nameIdx);
    public string getDest => TableManager.Instance.stringTable.GetText(destIdx);
}
public class StatRewardTable : TTableBase<StatRewardRecord>
{
    public StatRewardTable(ClassFileSave save, string path) : base(save, path)
    {

    }
}
