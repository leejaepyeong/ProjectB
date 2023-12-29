using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillRecord : RecordBase
{
    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public string iconPath;
    public eSkillType type;
    public eSkillDetailType detailType;
    public float coolTIme;
    public eSkillTarget targetType;
    public eDamagePerType damagePerType;
    public float damagePerValue;
    public int skillBulletTargetNum;
    public int equipRuneCount;
    public List<int> skillTags = new List<int>();
    private string eventNodePath;
    public EventGraph skillNode => BattleManager.Instance.ResourcePool.Load<EventGraph>(eventNodePath);
    public List<int> skillEffects = new List<int>();

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "Skill_Group");
        nameIdx = FileUtil.Get<int>(_data, "Skill_Name");
        destIdx = FileUtil.Get<int>(_data, "Skill_Desc");
        iconPath = FileUtil.Get<string>(_data, "Skill_IconPath");
        type = FileUtil.Get<eSkillType>(_data, "Skill_Type");
        detailType = FileUtil.Get<eSkillDetailType>(_data, "Skill_DetailType");
        coolTIme = FileUtil.Get<float>(_data, "Skill_Cooltime");
        targetType = FileUtil.Get<eSkillTarget>(_data, "Skill_Target");
        damagePerType = FileUtil.Get<eDamagePerType>(_data, "Skill_Dmg_Type");
        damagePerValue = FileUtil.Get<float>(_data, "Skill_Dmg_num");
        skillBulletTargetNum = FileUtil.Get<int>(_data, "Skill_TargetCount");
        equipRuneCount = FileUtil.Get<int>(_data, "Skill_Equip_Rune");
        for (int i = 0; i < 5; i++)
        {
            skillTags.Add(FileUtil.Get<int>(_data, $"Tag{i + 1}"));
        }
        eventNodePath = FileUtil.Get<string>(_data, "SkillNode");
        SetSkillEffectList(FileUtil.Get<string>(_data, "SkillEffects"));
    }

    private void SetSkillEffectList(string effectString)
    {
        if (string.IsNullOrEmpty(effectString) || effectString == "0") return;

        string[] effectSplits = effectString.Split('/');
        for (int i = 0; i < effectSplits.Length; i++)
        {
            skillEffects.Add(int.Parse(effectSplits[i]));
        }
    }

    public string getName => TableManager.Instance.stringTable.GetText(nameIdx);
    public string getDest => TableManager.Instance.stringTable.GetText(destIdx);

    public SkillRecord GetCopyRecord()
    {
        SkillRecord copy = new SkillRecord();
        copy.index = index;
        copy.groupIdx = groupIdx;
        copy.nameIdx = nameIdx;
        copy.destIdx = destIdx;
        copy.iconPath = iconPath;
        copy.type = type;
        copy.detailType = detailType;
        copy.coolTIme = coolTIme;
        copy.targetType = targetType;
        copy.damagePerType = damagePerType;
        copy.damagePerValue = damagePerValue;
        copy.skillBulletTargetNum = skillBulletTargetNum;
        copy.equipRuneCount = equipRuneCount;
        for (int i = 0; i < copy.skillTags.Count; i++)
        {
            copy.skillTags.Add(skillTags[i]);
        }
        copy.eventNodePath = eventNodePath;
        for (int i = 0; i < copy.skillEffects.Count; i++)
        {
            copy.skillEffects.Add(skillEffects[i]);
        }
        return copy;
    }

    #region Logic
    private float elaspedTime;
    public void UpdateFrame(float deltaTime)
    {
        if (elaspedTime <= 0) return;
        elaspedTime -= deltaTime;
    }
    public void SetCoolTime(float time = 0)
    {
        if (time == 0) elaspedTime = coolTIme;
        else elaspedTime = time;
    }
    public bool IsReadyCoolTime()
    {
        return elaspedTime <= 0;
    }
    #endregion
}
public class SkillTable : TTableBase<SkillRecord>
{
    public SkillTable(ClassFileSave save, string path) : base(save, path)
    {

    }
}
