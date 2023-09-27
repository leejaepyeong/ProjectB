using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringRecord : RecordBase
{
    public string kor;
    public string eng;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        kor = FileUtil.Get<string>(_data, "kor");
        eng = FileUtil.Get<string>(_data, "eng");
    }
}
public class StringTable : TTableBase<StringRecord>
{
    public StringTable(ClassFileSave fileSave, string path) : base(fileSave, path)
    {
    }

    public string GetText(int index)
    {
        StringRecord record = GetRecord(index);

        return record.kor;
    }
}
