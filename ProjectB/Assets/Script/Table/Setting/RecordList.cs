using System.Collections.Generic;
using UnityEngine;
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