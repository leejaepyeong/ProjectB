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

    public Data.SkillInfoData GetSkillData()
    {
        if (Data.DataManager.Instance.SkillInfoData.TryGet(index, out var data) == false) return null;

        Data.SkillInfoData skillData = new Data.SkillInfoData(data.Seed, data.SkillGroupSeed, data.Type, data.DetailType, data.NameIdx, data.DestIdx, data.CoolTIme, data.TargetType, data.DamagePerType, data.DamagePerValue, 
            data.EquipRuneCount, data.SkillTag1, data.SkillTag2, data.SkillTag3, data.SkillTag4, data.SkillTag5, data.EventNodePath);

        return skillData;
    }
    public Data.RuneInfoData GetRuneData(int runeId)
    {
        int seed = SaveData_PlayerSkill.Instance.equipRuneInfo[runeId].runeSeed;
        Data.DataManager.Instance.RuneInfoData.TryGet(seed, out var data); 
        return data;
    }
}
[System.Serializable]
public class RuneSaveInfo
{
    public bool isEquip;
    public int id;
    public int runeSeed;

    public RuneSaveInfo(int id, int seed)
    {
        this.id = id;
        this.runeSeed = seed;
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
        return equipSkillInfo[slotIdx];
    }

    public void SetRune(RuneSaveInfo saveData)
    {
        runeSaveInfoGroup.Add(saveData);
        RuneId += 1;

        SetChange();
        SetNotify();
    }

    public RuneSaveInfo GetRunInfo(int id)
    {
        return runeSaveInfoGroup.Find(a => a.id == id);
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
    public void RuneEquip(int skillIdx, int equipIdx, int runeId)
    {
        RuneUnEquip(skillIdx, equipIdx);

        var skillData = skillSaveInfoGroup.Find(a => a.index == skillIdx);
        skillData.equipRuneGroup[equipIdx] = runeId;

        var runeData = runeSaveInfoGroup.Find(a => a.id == runeId);
        runeData.isEquip = true;

        SetChange();
        SetNotify();
    }
    public void RuneUnEquip(int skillIdx, int equipIdx)
    {
        var skillData = skillSaveInfoGroup.Find(a => a.index == skillIdx);
        if (skillData.equipRuneGroup[equipIdx] == 0) return;

        var runeData = runeSaveInfoGroup.Find(a => a.id == skillData.equipRuneGroup[equipIdx]);
        runeData.isEquip = true;
        skillData.equipRuneGroup[equipIdx] = 0;

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
