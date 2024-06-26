using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillEffectRecord : RecordBase
{
    public class SkillEffectStat
    {
        public eStat stat;
        public eAddType addType;
        public float value;

        public SkillEffectStat(eStat stat, eAddType addType, float value)
        {
            this.stat = stat;
            this.addType = addType;
            this.value = value;
        }
    }

    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public eSkillTag skillTag;
    public eBuffType buffType;
    public eSkillState skillState;
    public List<eStat> skillStat;
    public List<eAddType> addType;
    public List<float> skillValue;
    public eSkillDuration skillDurationType;
    public float skillDuration;
    public bool isMultiAble;

    public List<SkillEffectStat> skillEffectStatList = new List<SkillEffectStat>();

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "SkillEffect_Group");
        nameIdx = FileUtil.Get<int>(_data, "SkillEffect_Name");
        destIdx = FileUtil.Get<int>(_data, "SkillEffect_Desc");
        buffType = FileUtil.Get<eBuffType>(_data, "SkillEffect_Type");
        skillState = FileUtil.Get<eSkillState>(_data, "SkillEffect_State");
        skillStat = FileUtil.GetList<eStat>(_data, "SkillEffect_StatType");
        addType = FileUtil.GetList<eAddType>(_data, "SkillEffect_AddType");
        skillValue = FileUtil.GetList<float>(_data, "SkillEffect_Factor");
        skillDurationType = FileUtil.Get<eSkillDuration>(_data, "SkillEffect_DurType");
        skillDuration = FileUtil.Get<float>(_data, "SkillEffect_Duration");

        if (skillStat.Count != addType.Count || skillStat.Count != skillValue.Count)
        {
            Debug.LogError("No Match Count in SkillEffectStat Info");
            return;
        }

        skillEffectStatList.Clear();
        for (int i = 0; i < skillStat.Count; i++)
        {
            skillEffectStatList.Add(new SkillEffectStat(skillStat[i], addType[i], skillValue[i]));
        }
    }

    public string GetName => TableManager.Instance.stringTable.GetText(nameIdx);
    public string GetDest => TableManager.Instance.stringTable.GetText(destIdx);
}
public class SkillEffectTable : TTableBase<SkillEffectRecord>
{
    public SkillEffectTable(ClassFileSave save, string path) : base(save, path)
    {

    }
}
