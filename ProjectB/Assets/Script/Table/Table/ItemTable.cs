using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRecord : RecordBase
{
    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public string iconPath;
    public eItemType itemType;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "Item_Group");
        nameIdx = FileUtil.Get<int>(_data, "Item_Name");
        destIdx = FileUtil.Get<int>(_data, "Item_Desc");
        iconPath = FileUtil.Get<string>(_data, "Item_IconPath");
        itemType = FileUtil.Get<eItemType>(_data, "Item_Type");
    }

    public string getName => TableManager.Instance.stringTable.GetText(nameIdx);
    public string getDest => TableManager.Instance.stringTable.GetText(destIdx);
}

public class ItemTable : TTableBase<ItemRecord>
{
    private Dictionary<int, ItemRecord> dicItem = new Dictionary<int, ItemRecord>();
    public ItemTable(ClassFileSave save, string path) : base(save, path)
    {

    }

    public override void Load()
    {
        base.Load();
        dicItem.Clear();
        for (int i = 0; i < getRecordList.Count; i++)
        {
            dicItem.Add(getRecordList[i].index, getRecordList[i]);
        }
    }
}
