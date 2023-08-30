using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISaveDataControl
{
    System.Type GetSaveType();
    SaveData GetSaveData();
    bool IsFileSave();
    void Update();
    void Load();
    void Init();
    bool IsNeedInit();
}

public class SaveDataControl<T> : ISaveDataControl where T : SaveData, new()
{
    protected System.Type saveType;
    protected string path = null;
    protected T saveData;
    protected StringFileSave fileSave = null;
    protected bool isNeedInit = true;

    public SaveDataControl(string path, StringFileSave fileSave, bool isNeedInit = true)
    {
        saveType = typeof(T);
        this.path = path;
        saveData = new T();
        this.fileSave = fileSave;
        this.isNeedInit = isNeedInit;
    }

    public SaveDataControl()
    {
        saveType = typeof(T);
        saveData = new T();
    }

    public SaveData GetSaveData()
    {
        return saveData;
    }

    public Type GetSaveType()
    {
        return saveType;
    }

    public void Init()
    {
        saveData = new T();
    }

    public bool IsFileSave()
    {
        return fileSave != null;
    }

    public bool IsNeedInit()
    {
        return isNeedInit;
    }

    public void Save()
    {
        if (fileSave == null) return;

        try
        {
            string data = JsonUtility.ToJson(saveData);
            if (data != null)
                fileSave.Save(path, data);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex + "save error : " + path);
        }
    }
    public void Load()
    {
        string data = "";

        if (fileSave == null) return;
        if(File.Exists(path) == false)
        {
            saveData = new T();
            data = JsonUtility.ToJson(saveData);
            if (data != null)
                fileSave.Save(path, data);
        }

        try
        {
            data = fileSave.Load(path);
            if(data != null)
            {
                saveData = JsonUtility.FromJson<T>(data);
                saveData.SetUp();
            }
        }
        catch
        {
            Debug.LogError("load error : " + path);
        }
    }

    public void Update()
    {
        if (saveData == null) return;

        saveData.UpdateFrame();
        if(saveData.IsChange)
        {
            if (fileSave != null)
                Save();
            saveData.SetChange(false);
        }
    }
}


public class SaveData : Subject
{
    protected bool isChange = false;

    public bool IsChange { get { return isChange; } }
    public void SetChange(bool isChange = true)
    {
        this.isChange = isChange;
    }

    public virtual void SetUp()
    {

    }
}
