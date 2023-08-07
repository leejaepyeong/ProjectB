using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public string nickName;
    public DateTime createDate;
}

public class SaveData_Local : SaveData
{
    public static SaveData_Local Instance
    {
        get
        {
            return Manager.Instance.getFileData.GetSaveData<SaveData_Local>();
        }
    }

    private UserInfo userInfo = new();
    public UserInfo UserInfo => userInfo;

    private bool isSaveData;
    public bool IsSaveData;

    public void SetUserInfo(UserInfo info)
    {
        userInfo.nickName = info.nickName;
        userInfo.createDate = info.createDate;
        isSaveData = true;

        SetChange();
        SetNotify();
    }

    public void ChangeNickName(string name)
    {
        userInfo.nickName = name;

        SetChange();
        SetNotify();
    }
}
