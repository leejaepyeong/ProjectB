using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuneTypeInfo
{
    public eRuneType runeType;
    public float value;
    public int runeTag;

    public RuneTypeInfo(eRuneType runeType, float value, int runeTag)
    {
        this.runeType = runeType;
        this.value = value;
        this.runeTag = runeTag;
    }
}
[Serializable]
public class RuneRecord : RecordBase
{
    public int groupIdx;
    public int nameIdx;
    public int destIdx;
    public string iconPath;
    public List<RuneTypeInfo> runeTypeInfoList = new List<RuneTypeInfo>();

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "Rune_Group");
        nameIdx = FileUtil.Get<int>(_data, "Rune_Name");
        destIdx = FileUtil.Get<int>(_data, "Rune_Desc");
        iconPath = FileUtil.Get<string>(_data, "Rune_IconPath");
        for (int i = 0; i < 3; i++)
        {
            runeTypeInfoList.Add(new RuneTypeInfo(FileUtil.Get<eRuneType>(_data, $"Rune_effect{i+1}"), FileUtil.Get<float>(_data, $"Rune_effect{i + 1}_Value"), FileUtil.Get<int>(_data, $"Rune_Tag{ i + 1 }")));
        }
    }
    public RuneRecord GetCopyRecord()
    {
        RuneRecord copy = new RuneRecord();
        copy.groupIdx = groupIdx;
        copy.nameIdx = nameIdx;
        copy.destIdx = destIdx;
        for (int i = 0; i < runeTypeInfoList.Count; i++)
        {
            copy.runeTypeInfoList[i] = new RuneTypeInfo(runeTypeInfoList[i].runeType, runeTypeInfoList[i].value, runeTypeInfoList[i].runeTag);
        }

        return copy;
    }

    public void AddSkillEffectToSkill(List<SkillEffectRecord> skillEffectList)
    {
        for (int i = 0; i < runeTypeInfoList.Count; i++)
        {
            if (runeTypeInfoList[i].runeType != eRuneType.AddEffect) continue;
            if (TableManager.Instance.skillEffectTable.TryGetRecord((int)runeTypeInfoList[i].value, out var record))
            {
                var temp = record.GetCopyRecord();
                skillEffectList.Add(temp);
            }
        }
    }
    public void SetRuneEffectToSkill(SkillRecord skill)
    {
        if (UnitManager.Instance.Player == null)
        {
            Debug.LogError("Player is Null");
            return;
        }
        var player = UnitManager.Instance.Player;

        for (int i = 0; i < runeTypeInfoList.Count; i++)
        {
            switch (runeTypeInfoList[i].runeType)
            {
                case eRuneType.CoolTimeDown:
                    skill.coolTIme -= runeTypeInfoList[i].value;
                    break;
                case eRuneType.AddProjectilenum:
                    skill.skillBulletTargetNum += (int)runeTypeInfoList[i].value;
                    break;
                case eRuneType.AddProjectilesize:
                    skill.skillBulletSize += runeTypeInfoList[i].value;
                    break;
                case eRuneType.AddProjectilespd:
                    skill.skillBulletSpd += runeTypeInfoList[i].value;
                    break;
                case eRuneType.AddProjectiledmg:
                    skill.damagePerValue += runeTypeInfoList[i].value;
                    break;
                case eRuneType.MinProjectilenum:
                    skill.skillBulletTargetNum -= (int)runeTypeInfoList[i].value;
                    break;
                case eRuneType.MinProjectilesize:
                    skill.skillBulletSize -= runeTypeInfoList[i].value;
                    break;
                case eRuneType.MinProjectilespd:
                    skill.skillBulletSpd -= runeTypeInfoList[i].value;
                    break;
                case eRuneType.MinProjectiledmg:
                    skill.damagePerValue -= runeTypeInfoList[i].value;
                    break;
                case eRuneType.AddRange:
                    player.UnitState.AddStat(eStat.atkRange, runeTypeInfoList[i].value);
                    break;
                case eRuneType.AddTarget:
                    skill.skillBulletTargetNum += (int)runeTypeInfoList[i].value;
                    break;
                case eRuneType.AddAtk:
                    player.UnitState.AddStat(eStat.atk, runeTypeInfoList[i].value);
                    break;
                case eRuneType.AddAtkspd:
                    player.UnitState.AddStat(eStat.atkSpd, runeTypeInfoList[i].value);
                    break;
                case eRuneType.AddCrirate:
                    player.UnitState.AddStat(eStat.criRate, runeTypeInfoList[i].value);
                    break;
                case eRuneType.AddCridmg:
                    player.UnitState.AddStat(eStat.criDmg, runeTypeInfoList[i].value);
                    break;
                case eRuneType.MinAtk:
                    player.UnitState.AddStat(eStat.atk, -runeTypeInfoList[i].value);
                    break;
                case eRuneType.MinAtkspd:
                    player.UnitState.AddStat(eStat.atkSpd, -runeTypeInfoList[i].value);
                    break;
                case eRuneType.MinCrirate:
                    player.UnitState.AddStat(eStat.criRate, -runeTypeInfoList[i].value);
                    break;
                case eRuneType.MinCridmg:
                    player.UnitState.AddStat(eStat.criDmg, -runeTypeInfoList[i].value);
                    break;
                case eRuneType.GetBonusExp:
                    break;
                default:
                    break;
            }
        }
    }
    public void SetRuneEffectToSkillEffect(SkillEffectRecord skillEffect)
    {
        for (int i = 0; i < runeTypeInfoList.Count; i++)
        {
            switch (runeTypeInfoList[i].runeType)
            {
                case eRuneType.AddEffectTime:
                    skillEffect.skillDuration += runeTypeInfoList[i].value;
                    break;
                case eRuneType.AddDmg:
                    skillEffect.skillValue += runeTypeInfoList[i].value;
                    break;
                case eRuneType.MinDmg:
                    skillEffect.skillValue -= runeTypeInfoList[i].value;
                    break;
                default:
                    break;
            }
        }
    }
}
public class RuneTable : TTableBase<RuneRecord>
{
    public RuneTable(ClassFileSave save, string path) : base(save, path)
    {

    }
}
