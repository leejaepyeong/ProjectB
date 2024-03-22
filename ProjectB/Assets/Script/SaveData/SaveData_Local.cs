using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInfo
{
    public string nickName;
    public string createDate;

    public UserInfo(string nick)
    {
        nickName = nick;
        createDate = DateTime.Now.ToString();
    }
}

[System.Serializable]
public class UserStat
{
    public long hp;
    public long mp;
    public long atk;
    public long def;
    public float acc;
    public float dod;
    public float atkRange;
    public float moveSpd;
    public float atkSpd;
    public float criRate;
    public float criDmg;
}

[System.Serializable]
public class SaveData_Local : SaveData
{
    public static SaveData_Local Instance
    {
        get
        {
            return Manager.Instance.getFileData.GetSaveData<SaveData_Local>();
        }
    }

    public UserInfo userInfo = new UserInfo("");
    public UserStat userStat = new UserStat();
    public bool isSaveData;
    
    #region UserInfo
    public void SetUserInfo(UserInfo info)
    {
        userInfo.nickName = info.nickName;
        userInfo.createDate = info.createDate;
        isSaveData = true;
        SetUserStat();

        SetChange();
        SetNotify();
    }

    public void ChangeNickName(string name)
    {
        userInfo.nickName = name;

        SetChange();
        SetNotify();
    }
    #endregion

    #region UserStat
    public void SetUserStat()
    {
        userStat.hp = 100;
        userStat.mp = 100;
        userStat.atk = 20;
        userStat.def = 0;
        userStat.acc = 85;
        userStat.dod = 0;
        userStat.atkSpd = 1;
        userStat.atkRange = 10;
        userStat.criRate = 25;
        userStat.criDmg = 100;

        SetChange();
        SetNotify();
    }

    public void ChangeUserStat(eStat stat, float FValue = 0f, long LValue = 0)
    {
        switch (stat)
        {
            case eStat.hp:
                userStat.hp = LValue;
                break;
            case eStat.mp:
                userStat.mp = LValue;
                break;
            case eStat.atk:
                userStat.atk = LValue;
                break;
            case eStat.def:
                userStat.def = LValue;
                break;
            case eStat.acc:
                userStat.acc = FValue;
                break;
            case eStat.dod:
                userStat.dod = FValue;
                break;
            case eStat.atkSpd:
                userStat.atkSpd = FValue;
                break;
            case eStat.atkRange:
                userStat.atkRange = FValue;
                break;
            case eStat.criRate:
                userStat.criRate = FValue;
                break;
            case eStat.criDmg:
                userStat.criDmg = FValue;
                break;
        }

        SetChange();
        SetNotify();
    }
    #endregion
}
