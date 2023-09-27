using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuneTypeInfo
{
    public eRuneType runeType;
    public float value;
    public int runeTag;

    public RuneTypeInfo(eRuneType runeType, float value, int runeTag)
    {
        this.runeType = runeType;
        this.value = value;
        this.runeTag = runeTag;
    }
}
[Serializable]
public class RuneRecord : RecordBase
{
    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public List<RuneTypeInfo> runTypeInfoList = new List<RuneTypeInfo>();

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "Rune_Group");
        nameIdx = FileUtil.Get<int>(_data, "Rune_Name");
        destIdx = FileUtil.Get<int>(_data, "Rune_Desc");
        for (int i = 0; i < 3; i++)
        {
            runTypeInfoList.Add(new RuneTypeInfo(FileUtil.Get<eRuneType>(_data, $"Rune_effect{i+1}"), FileUtil.Get<float>(_data, $"Rune_effect{i + 1}_Value"), FileUtil.Get<int>(_data, $"Rune_Tag{ i + 1 }")));
        }
    }
}
public class RuneTable : TTableBase<RuneRecord>
{
    public RuneTable(ClassFileSave save, string path) : base(save, path)
    {

    }
}
