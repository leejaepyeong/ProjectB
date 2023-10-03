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
    public int tag;
    public eSkillState skillState;
    public eBuffType buffType;
    public float skillValue;
    public float skillDuration;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "SkillEffect_Group");
        nameIdx = FileUtil.Get<int>(_data, "SkillEffect_Name");
        destIdx = FileUtil.Get<int>(_data, "SkillEffect_Desc");
        tag = FileUtil.Get<int>(_data, "SkillEffect_Tag");
        skillState = FileUtil.Get<eSkillState>(_data, "SkillEffect_State");
        buffType = FileUtil.Get<eBuffType>(_data, "SkillEffect_Buff");
        skillValue = FileUtil.Get<float>(_data, "SkillEffect_Value");
        skillDuration = FileUtil.Get<float>(_data, "SkillEffect_Duration");
    }

    public SkillEffectRecord GetCopyRecord()
    {
        SkillEffectRecord copy = new SkillEffectRecord();
        copy.groupIdx = groupIdx;
        copy.nameIdx = nameIdx;
        copy.destIdx = destIdx;
        copy.tag = tag;
        copy.skillState = skillState;
        copy.buffType = buffType;
        copy.skillValue = skillValue;
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
