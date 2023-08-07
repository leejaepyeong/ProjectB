using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestRecord : RecordBase
{
    private string Korean;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        Korean = FileUtil.Get<string>(_data, "String_kr");
    }

    public string GetText()
    {
        return Korean;
    }
}


public class TestTable : TTableBase<TestRecord>
{
    public TestTable(string _path, ClassFileSave _fileSave) : base(_path, _fileSave)
    {
    }

    public virtual string GetText(int index)
    {
        TestRecord record = GetRecord(index);

        return record.GetText();
    }
}
