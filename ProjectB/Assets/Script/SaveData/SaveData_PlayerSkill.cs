using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData_PlayerSkill : SaveData
{
    public static SaveData_PlayerSkill Instance
    {
        get
        {
            return Manager.Instance.getFileData.GetSaveData<SaveData_PlayerSkill>();
        }
    }
}
