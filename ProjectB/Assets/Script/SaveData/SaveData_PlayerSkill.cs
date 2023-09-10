using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSaveInfo
{
    public bool isInit;
    public bool isHave;
    public int lv;
    public int index;
    public int equipIndex;
    public int[] equipRuneGroup = new int[10];

    public void Init(int idx)
    {
        isInit = true;
        isHave = false;
        lv = 1;
        index = idx;
        equipIndex = -1;
    }
}
[System.Serializable]
public class RuneSaveInfo
{
    public bool isEquip;
    public int id;
    public int runeIdx;

    public RuneSaveInfo(int id, int runeIdx)
    {
        this.id = id;
        this.runeIdx = runeIdx;
    }
}
public class SaveData_PlayerSkill : SaveData
{
    public SkillSaveInfo[] skillSaveInfoGroup = new SkillSaveInfo[999];
    public List<RuneSaveInfo> runeSaveInfoGroup = new List<RuneSaveInfo>();
    public static SaveData_PlayerSkill Instance
    {
        get
        {
            return Manager.Instance.getFileData.GetSaveData<SaveData_PlayerSkill>();
        }
    }

    #region SkillInfo
    public void SetSkillInfo()
    {
        for (int i = 0; i < skillSaveInfoGroup.Length; i++)
        {
            if (skillSaveInfoGroup[i].isInit == false)
                skillSaveInfoGroup[i].Init(i + 1);
        }

        RuneSaveInfo info = new RuneSaveInfo(1,2);
        SetRune(info);

        SetChange();
        SetNotify();
    }

    public void SetSKill(int idx)
    {
        skillSaveInfoGroup[idx].isHave = true;
    }

    public SkillSaveInfo GetSkill(int idx)
    {
        return skillSaveInfoGroup[idx];
    }

    public void SetRune(RuneSaveInfo saveData)
    {
        runeSaveInfoGroup.Add(saveData);
    }

    public RuneSaveInfo GetRunInfo(int id)
    {
        return runeSaveInfoGroup.Find(a => a.id == id);
    }

    #endregion

    #region SkillEquip

    public void ChangeSkillEquip(int slotIdx, int skillIdx)
    {
        skillSaveInfoGroup[skillIdx].equipIndex = slotIdx;

        SetChange();
        SetNotify();
    }
    public void ChangeSkillIRune(int skillIdx, int equipIdx, int runeId)
    {
        skillSaveInfoGroup[skillIdx].equipRuneGroup[equipIdx] = runeId;

        SetChange();
        SetNotify();
    }
    public void ChangeSkillLevel(int skillIdx, int value)
    {
        skillSaveInfoGroup[skillIdx].lv = value;

        SetChange();
        SetNotify();
    }
    #endregion
}
