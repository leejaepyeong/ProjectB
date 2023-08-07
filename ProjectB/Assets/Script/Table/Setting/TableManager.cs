using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager
{
    #region table
    public TestTable testTable;
    #endregion

    private ClassFileSave fileSave = new();
    private List<TableBase> tableList = new();

    public ClassFileSave getFileSave
    {
        get { return fileSave;}
    }

    public void Load(ClassFileSave fileSave = null)
    {
        this.fileSave = fileSave;
        tableList.Clear();

        tableList.Add(testTable = new TestTable("Table/TestTable", fileSave));

        for (int i = 0; i < tableList.Count; i++)
        {
            tableList[i].Load();
        }

    }
}
