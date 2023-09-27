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

    public TTableBase(string path, ClassFileSave fileSave)
        : base(path, fileSave)
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
        m_recordList.Clear();
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
            Debug.LogWarning(typeof(T).ToString() + " : " + _index.ToString());
        }
        return _record;
    }

    public bool IsHasRecord(int _index)
    {
        return m_recordList.IsHasRecord(_index);
    }
}

public class RecordList<T> where T : RecordBase, new()
{
    public class ComparerRecord : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            return x.index.CompareTo(y.index);
        }
    }

    protected static int ComparisonRecord(RecordBase _r1, RecordBase _r2)
    {
        return _r1.index.CompareTo(_r2.index);
    }

    protected List<T> m_recordList = new List<T>();
    protected T m_search = new T();
    protected ComparerRecord m_compareRecord = new ComparerRecord();

    public List<T> getRecordList
    {
        get
        {
            return m_recordList;
        }
    }

    public void Clear()
    {
        if (null == m_recordList)
            return;
        m_recordList.Clear();
    }

    public void SetRecordList(List<T> _recordList)
    {
        if (null == _recordList)
            return;


        m_recordList = _recordList;
        m_recordList.Sort(ComparisonRecord);
    }

    public void AddRecordList(List<T> _recordList)
    {
        if (null == _recordList)
            return;
        m_recordList.AddRange(_recordList);
        m_recordList.Sort(ComparisonRecord);
    }
    public T AddRecord(int _index)
    {
        if (null == m_recordList)
            m_recordList = new List<T>();

        T _find = GetRecord(_index);
        if (null == _find)
        {
            _find = new T();
            _find.index = _index;
            m_recordList.Add(_find);
            Sort();
        }
        return _find;
    }

    public void Sort()
    {
        m_recordList.Sort(ComparisonRecord);
    }
    public T GetRecord(int _index)
    {
        if (null == m_recordList)
            return null;

        m_search.index = _index;
        int _searchIndex = m_recordList.BinarySearch(m_search, m_compareRecord);
        if (_searchIndex < 0)
        {
            return null;
        }
        return m_recordList[_searchIndex];
    }
    public bool IsHasRecord(int _index)
    {
        if (m_recordList == null)
            return false;

        m_search.index = _index;
        int _searchIndex = m_recordList.BinarySearch(m_search, m_compareRecord);
        return _searchIndex >= 0;
    }
}
