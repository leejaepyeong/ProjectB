using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eKey
{
    //InGame
    Skill_1 = 1001,
    Skill_2,
    Skill_3,
    Skill_4,
    Skill_5,
    Option,
    Inventory,
    Stat,
}

[System.Serializable]
public class Option_SoundInfo
{
    public int masterVolume;
    public int bgmVolume;
    public int effectVolume;

    public bool masterMute;
    public bool bgmMute;
    public bool effectMute;

    public void Init()
    {
        masterVolume = 100;
        bgmVolume = 100;
        effectVolume = 100;

        masterMute = false;
        bgmMute = false;
        effectMute = false;
    }
}
[System.Serializable]
public class Option_KeySettingInfo
{
    public Dictionary<eKey, KeyCode> keySetting = new Dictionary<eKey, KeyCode>();

    public void Init()
    {
        keySetting.Clear();
        
        keySetting.Add(eKey.Option, KeyCode.Escape);
        keySetting.Add(eKey.Inventory, KeyCode.I);
        keySetting.Add(eKey.Stat, KeyCode.S);

        keySetting.Add(eKey.Skill_1, KeyCode.Alpha1);
        keySetting.Add(eKey.Skill_1, KeyCode.Alpha2);
        keySetting.Add(eKey.Skill_1, KeyCode.Alpha3);
        keySetting.Add(eKey.Skill_1, KeyCode.Alpha4);
        keySetting.Add(eKey.Skill_1, KeyCode.Alpha5);

    }
}
[System.Serializable]
public class SaveData_Option : SaveData
{
    public static SaveData_Option Instance
    {
        get
        {
            return Manager.Instance.getFileData.GetSaveData<SaveData_Option>();
        }
    }

    public Option_SoundInfo optionSoundInfo = new();
    public Option_KeySettingInfo optionkeySettingInfo = new();

    public bool isSaveData;

    public void SetBasicOption()
    {
        SetSound();
        SetKeySetting();
    }

    #region Sound
    public void SetSound()
    {
        optionSoundInfo.Init();

        SetChange();
        SetNotify();
    }
    public void ChangeSound()
    {

    }
    #endregion
    #region Sound
    public void SetKeySetting()
    {
        optionkeySettingInfo.Init();

        SetChange();
        SetNotify();
    }
    public void ChangeKeySetting(eKey key, KeyCode keyCode)
    {
        optionkeySettingInfo.keySetting[key] = keyCode;
        SetChange();
        SetNotify();
    }
    #endregion
}
