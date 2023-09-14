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

    public Data.SkillInfoData getSkillData { get { Data.DataManager.Instance.SkillInfoData.TryGet(index, out var data); return data; } }
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
        isEquip = false;
    }
}
public class SaveData_PlayerSkill : SaveData
{
    //ÀüÃ¼ ½ºÅ³, ·é
    public List<SkillSaveInfo> skillSaveInfoGroup = new List<SkillSaveInfo>();
    public List<RuneSaveInfo> runeSaveInfoGroup = new List<RuneSaveInfo>();
    //ÀåÂø ½ºÅ³, ·é
    public List<SkillSaveInfo> equipSkillInfo = new List<SkillSaveInfo>(Define.MaxEquipSkill);
    public Dictionary<int, RuneSaveInfo> equipRuneInfo = new Dictionary<int, RuneSaveInfo>();
    public static SaveData_PlayerSkill Instance
    {
        get
        {
            return Manager.Instance.getFileData.GetSaveData<SaveData_PlayerSkill>();
        }
    }

    public void Init()
    {
        for (int i = 0; i < runeSaveInfoGroup.Count; i++)
        {
            if(runeSaveInfoGroup[i].isEquip)
                equipRuneInfo.Add(runeSaveInfoGroup[i].id, runeSaveInfoGroup[i]);
        }
    }

    #region SkillInfo
    public void SetSkillInfo()
    {
        for (int i = 0; i < Data.DataManager.Instance.SkillInfoData.DataList.Count; i++)
        {
            var data = Data.DataManager.Instance.SkillInfoData.DataList[i];

            if (skillSaveInfoGroup.Count <= i)
                skillSaveInfoGroup.Add(new SkillSaveInfo());
            if (skillSaveInfoGroup[i].isInit) continue;
                
            skillSaveInfoGroup[i].Init(data.Seed);
        }

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
        var preSkillData = skillSaveInfoGroup.Find(a => a.equipIndex == slotIdx);
        if (preSkillData != null) preSkillData.equipIndex = -1;

        var skillData = skillSaveInfoGroup.Find(a => a.index == skillIdx);
        skillData.equipIndex = slotIdx;
        equipSkillInfo[slotIdx] = skillData;

        SetChange();
        SetNotify();
    }
    public void ChangeSkillIRune(int skillIdx, int equipIdx, int runeId)
    {
        var skillData = skillSaveInfoGroup.Find(a => a.index == skillIdx);
        skillData.equipRuneGroup[equipIdx] = runeId;

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
