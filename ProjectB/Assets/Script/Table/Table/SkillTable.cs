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
    public eSkillType type;
    public eSkillDetailType detailType;
    public float coolTIme;
    public eSkillTarget targetType;
    public eDamagePerType damagePerType;
    public float damagePerValue;
    public int skillBulletTargetNum;
    public float skillBulletSpd;
    public float skillBulletSize;
    public int equipRuneCount;
    public int[] skillTags = new int[5];
    public int skillTag1;
    public int skillTag2;
    public int skillTag3;
    public int skillTag4;
    public int skillTag5;
    private string eventNodePath;
    public EventGraph skillNode => BattleManager.Instance.ResourcePool.Load<EventGraph>(eventNodePath);

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "Skill_Group");
        nameIdx = FileUtil.Get<int>(_data, "Skill_Type");
        destIdx = FileUtil.Get<int>(_data, "Skill_DetailType");
        type = FileUtil.Get<eSkillType>(_data, "Skill_Name");
        detailType = FileUtil.Get<eSkillDetailType>(_data, "Skill_Desc");
        coolTIme = FileUtil.Get<float>(_data, "Skill_Cooltime");
        targetType = FileUtil.Get<eSkillTarget>(_data, "Skill_Target");
        damagePerType = FileUtil.Get<eDamagePerType>(_data, "Skill_Dmg_Type");
        damagePerValue = FileUtil.Get<float>(_data, "Skill_Dmg_num");
        skillBulletTargetNum = FileUtil.Get<int>(_data, "Skill_Bullet");
        skillBulletSpd = FileUtil.Get<int>(_data, "Skill_Bullet_Spd");
        skillBulletSize = FileUtil.Get<int>(_data, "Skill_Bullet_Size");
        equipRuneCount = FileUtil.Get<int>(_data, "Skill_Equip_Rune");
        for (int i = 0; i < skillTags.Length; i++)
        {
            skillTags[i] = FileUtil.Get<int>(_data, $"Tag{i + 1}");
        }
        eventNodePath = FileUtil.Get<string>(_data, "EventNode");
    }
}
public class SkillTable : TTableBase<SkillRecord>
{
    public SkillTable(ClassFileSave save, string path) : base(path, save)
    {

    }
}
