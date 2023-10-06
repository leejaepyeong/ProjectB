using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillEffectRecord : RecordBase
{
    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public eBuffType buffType;
    public eSkillState skillState;
    public eStat skillStat;
    public eAddType addType;
    public float skillValue;
    public eSkillTarget skillTarget;
    public eSkillDuration skillDurationType;
    public float skillDuration;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "SkillEffect_Group");
        nameIdx = FileUtil.Get<int>(_data, "SkillEffect_Name");
        destIdx = FileUtil.Get<int>(_data, "SkillEffect_Desc");
        buffType = FileUtil.Get<eBuffType>(_data, "SkillEffect_Type");
        skillState = FileUtil.Get<eSkillState>(_data, "SkillEffect_State");
        skillStat = FileUtil.Get<eStat>(_data, "SkillEffect_StatType");
        addType = FileUtil.Get<eAddType>(_data, "SkillEffect_AddType");
        skillValue = FileUtil.Get<float>(_data, "SkillEffect_Factor");
        skillTarget = FileUtil.Get<eSkillTarget>(_data, "SkillEffect_Target");
        skillDurationType = FileUtil.Get<eSkillDuration>(_data, "SkillEffect_DurType");
        skillDuration = FileUtil.Get<float>(_data, "SkillEffect_Duration");
    }

    public SkillEffectRecord GetCopyRecord()
    {
        SkillEffectRecord copy = new SkillEffectRecord();
        copy.groupIdx = groupIdx;
        copy.nameIdx = nameIdx;
        copy.destIdx = destIdx;
        copy.buffType = buffType;
        copy.skillState = skillState;
        copy.skillStat = skillStat;
        copy.addType = addType;
        copy.skillValue = skillValue;
        copy.skillTarget = skillTarget;
        copy.skillDurationType = skillDurationType;
        copy.skillDuration = skillDuration;

        return copy;
    }
}
public class SkillEffectTable : TTableBase<SkillEffectRecord>
{
    public SkillEffectTable(ClassFileSave save, string path) : base(save, path)
    {

    }
}
