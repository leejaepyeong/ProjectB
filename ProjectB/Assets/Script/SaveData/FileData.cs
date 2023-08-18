using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileData : MonoBehaviour
{
    public static FileData Instance
    {
        get
        {
            if(Manager.Instance == null)
                return null;
            return Manager.Instance.getFileData;
        }
    }

    private StringFileSave fileSave = new StringFileSave();
    protected List<ISaveDataControl> saveDataList = new();
    private bool isLoadDone;
    public bool IsLoadDone => isLoadDone;

    public void Init()
    {
        StringFileSave fileSave = new StringFileSave();

        saveDataList.Add(new SaveDataControl<SaveData_Local>(fileSave.GetPath("local"), fileSave));

        LoadFile();
    }

    protected void LoadFile()
    {
        for (int i = 0; i < saveDataList.Count; i++)
        {
            saveDataList[i].Load();
        }
    }

    public T GetSaveData<T>() where T : SaveData
    {
        System.Type key = typeof(T);
        ISaveDataControl saveData = saveDataList.Find(item => item.GetSaveType() == key);
        if(saveData == null)
        {
            Debug.LogError("error : " + typeof(T).ToString());
            return null;
        }
        return saveData.GetSaveData() as T;
    }

    public virtual void InitSaveData()
    {
        for (int i = 0; i < saveDataList.Count; i++)
        {
            if (saveDataList[i].IsFileSave() == false)
                saveDataList[i].Init();
        }

        isLoadDone = true;
    }
}
