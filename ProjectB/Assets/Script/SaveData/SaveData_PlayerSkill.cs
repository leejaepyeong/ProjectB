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

    public void Init(int idx)
    {
        isInit = true;
        isHave = false;
        lv = 1;
        index = idx;
        equipIndex = -1;
    }

    public SkillRecord GetSkillData()
    {
        if (TableManager.Instance.skillTable.TryGetRecord(index, out var data) == false) return null;
        return data.GetCopyRecord();
    }
}
public class SaveData_PlayerSkill : SaveData
{
    //ÀüÃ¼ ½ºÅ³, ·é
    public List<SkillSaveInfo> skillSaveInfoGroup = new List<SkillSaveInfo>();
    //ÀåÂø ½ºÅ³, ·é
    public List<SkillSaveInfo> equipSkillInfo = new List<SkillSaveInfo>();

    public int RuneId;
    public static SaveData_PlayerSkill Instance
    {
        get
        {
            return Manager.Instance.getFileData.GetSaveData<SaveData_PlayerSkill>();
        }
    }

    public void Init()
    {
    }

    #region SkillInfo
    public void SetSkillInfo()
    {
        for (int i = 0; i < TableManager.Instance.skillTable.getRecordList.Count; i++)
        {
            var data = TableManager.Instance.skillTable.getRecordList[i];

            if (skillSaveInfoGroup.Count <= i)
                skillSaveInfoGroup.Add(new SkillSaveInfo());
            if (skillSaveInfoGroup[i].isInit) continue;
                
            skillSaveInfoGroup[i].Init(data.index);
        }

        if (RuneId == 0) RuneId = 1;

        SetChange();
        SetNotify();
    }

    public void SetSKill(int idx)
    {
        skillSaveInfoGroup[idx].isHave = true;

        SetChange();
        SetNotify();
    }

    public SkillSaveInfo GetSkill(int idx)
    {
        return skillSaveInfoGroup[idx];
    }
    public SkillSaveInfo GetEquipSkill(int slotIdx)
    {
        if (equipSkillInfo.Count <= slotIdx) return null;

        return equipSkillInfo[slotIdx];
    }

    #endregion

    #region SkillEquip

    public void SkillEquip(int slotIdx, int skillIdx)
    {
        SkillUnEquip(slotIdx);

        var skillData = skillSaveInfoGroup.Find(a => a.index == skillIdx);
        skillData.equipIndex = slotIdx;
        equipSkillInfo[slotIdx] = skillData;

        SetChange();
        SetNotify();
    }
    public void SkillUnEquip(int slotIdx)
    {
        if (equipSkillInfo[slotIdx] == null) return;

        var preSkillData = skillSaveInfoGroup.Find(a => a.equipIndex == slotIdx);
        if (preSkillData != null) preSkillData.equipIndex = -1;

        equipSkillInfo[slotIdx] = null;
    }
    public void ChangeSkillLevel(int skillIdx, int value)
    {
        skillSaveInfoGroup[skillIdx].lv = value;

        SetChange();
        SetNotify();
    }
    #endregion
}
