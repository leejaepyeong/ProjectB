using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpRecord : RecordBase
{
    public int level;
    public int needExp;
    public int rewardIdx;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        level = FileUtil.Get<int>(_data, "Level");
        needExp = FileUtil.Get<int>(_data, "NeedExp");
        rewardIdx = FileUtil.Get<int>(_data, "RewardIndex");
    }
}

public class ExpTable : TTableBase<ExpRecord>
{
    public ExpTable(ClassFileSave save, string path) : base(save, path)
    {

    }

    public ExpRecord GetExpRecord(int exp)
    {
        ExpRecord record = null;
        for (int i = 0; i < getRecordList.Count; i++)
        {
            record = getRecordList[i];
            if (exp - record.needExp < 0) break;
            exp -= record.needExp;
        }

        return record;
    }
}
