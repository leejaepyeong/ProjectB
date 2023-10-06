using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitRecord : RecordBase
{
    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public eUnitType Type;

    public long hp;
    public long mp;
    public long atk;
    public long def;
    public float acc;
    public float atkSpd;
    public float moveSpd;
    public float atkRange;
    public float criRate;
    public float criDmg;
    public string iconPath;
    public string modelPath;
    public string animatorPath;
    public int atkIdx;
    public List<int> skillGroup = new List<int>();
    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "Unit_Group");
        nameIdx = FileUtil.Get<int>(_data, "Unit_Name");
        destIdx = FileUtil.Get<int>(_data, "Unit_Desc");
    }
}

public class UnitTable : TTableBase<UnitRecord>
{
    public UnitTable(ClassFileSave save, string path) : base(save, path)
    {

    }
}
