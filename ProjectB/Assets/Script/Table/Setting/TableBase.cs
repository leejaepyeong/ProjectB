using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecordBase
{
    public int index;

    public virtual void LoadExcel(Dictionary<string, string> _data)
    {
        index = FileUtil.Get<int>(_data, "index");
    }
}

public abstract class TableBase
{
    protected string m_path;
    protected ClassFileSave m_fileSave;

    public TableBase(string _path, ClassFileSave _fileSave)
    {
        m_path = _path;
        m_fileSave = _fileSave;
    }

    public string getPath
    {
        get
        {
            return m_path;
        }
    }
    public abstract void Load();
    public abstract void Write();
    public virtual void LoadExcel(string _sheetName, List<Dictionary<string, string>> _data) { }
}

public class TTableBase<T> : TableBase where T : RecordBase, new()
{
    protected RecordList<T> m_recordList = new RecordList<T>();
    public UnityEngine.Events.UnityAction<T, Dictionary<string, string>> m_loadExcelAction;

    public TTableBase(string _path, ClassFileSave _fileSave)
        : base(_path, _fileSave)
    {

    }

    public List<T> getRecordList
    {
        get
        {
            return m_recordList.getRecordList;
        }
    }

    public void Sort()
    {
        m_recordList.Sort();
    }

    public override void Load()
    {
        try
        {
            List<T> _list = (List<T>)m_fileSave.LoadRes(getPath);
            m_recordList.SetRecordList(_list);
        }
        catch (System.Exception e)
        {
            Debug.LogError("TableBase:Load() : " + getPath + " : " + e.ToString());
        }
    }

    public override void Write()
    {
        CheckSameIndex();
        //ResolveSameIndex();
        m_fileSave.Save(m_fileSave.GetResPath(getPath), m_recordList.getRecordList);
    }

    public virtual void CheckSameIndex()
    {
        Dictionary<int, T> _checkIndex = new Dictionary<int, T>();
        for (int i = 0; i < m_recordList.getRecordList.Count; ++i)
        {
            int _index = m_recordList.getRecordList[i].index;
            if (_checkIndex.ContainsKey(_index) == true)
            {
                Debug.LogError(typeof(T).ToString() + " [ same index : " + _index);
                continue;
            }
            _checkIndex.Add(_index, null);
        }
    }

    public virtual void ResolveSameIndex()
    {
        Dictionary<int, T> _checkIndex
           = new Dictionary<int, T>();
        for (int i = 0; i < m_recordList.getRecordList.Count; ++i)
        {
            int _index = m_recordList.getRecordList[i].index;
            if (_checkIndex.ContainsKey(_index) == true)
            {
                _checkIndex[_index] = m_recordList.getRecordList[i];
                continue;
            }
            _checkIndex.Add(_index, m_recordList.getRecordList[i]);
        }

        m_recordList.Clear();
        var _var = _checkIndex.GetEnumerator();
        List<T> _list = new List<T>();
        while (_var.MoveNext())
        {
            _list.Add(_var.Current.Value);
        }

        m_recordList.SetRecordList(_list);
    }

    public override void LoadExcel(string _sheetName, List<Dictionary<string, string>> _data)
    {
        List<T> _recordList = new List<T>();
        foreach (Dictionary<string, string> _var in _data)
        {
            T _record = new T();
            if (null != m_loadExcelAction)
                m_loadExcelAction(_record, _var);
            else
                _record.LoadExcel(_var);

            if (_record.index <= 0)
            {
                continue;
            }

            _recordList.Add(_record);
        }
        m_recordList.AddRecordList(_recordList);
    }

    public virtual T AddRecord(int _index)
    {
        return m_recordList.AddRecord(_index);
    }

    public T GetRecord(int _index)
    {
        T _record = m_recordList.GetRecord(_index);
        if (null == _record && typeof(T).ToString() != "StringRecord")
        {
            Debug.LogError(typeof(T).ToString() + " : " + _index.ToString());
        }
        return _record;
    }

    public bool IsHasRecord(int _index)
    {
        return m_recordList.IsHasRecord(_index);
    }
}