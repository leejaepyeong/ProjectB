using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpRecord : RecordBase
{
    public int level;
    public long needExp;
    public int rewardIdx;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        level = FileUtil.Get<int>(_data, "Level");
        needExp = FileUtil.Get<long>(_data, "NeedExp");
        rewardIdx = FileUtil.Get<int>(_data, "RewardIndex");
    }
}

public class ExpTable : TTableBase<ExpRecord>
{
    public ExpTable(ClassFileSave save, string path) : base(save, path)
    {

    }

    public ExpRecord GetExpRecord(long exp, out long remainExp)
    {
        remainExp = exp;
        ExpRecord record = null;
        for (int i = 0; i < getRecordList.Count; i++)
        {
            record = getRecordList[i];
            if (remainExp - record.needExp < 0) break;
            remainExp -= record.needExp;
        }

        return record;
    }
    public ExpRecord GetExpRecord(int level)
    {
        ExpRecord record = null;
        record = getRecordList.Find(_ => _.level == level);
        return record;
    }
}
