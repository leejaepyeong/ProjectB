using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessagePack;
using UnityEngine;
using UnityEditor;

namespace Data
{
    public interface IDataKey<E>
    {
        public E Key { get; }
    }

    [MessagePackObject(true)]
    public class ArrayMeta
    {
        public Dummy[] Dummy { get; }
        public StringText[] StringText { get; }
        public SkillInfoData[] SkillInfoData { get; }
    }
    [MessagePackObject(true)]
    public class ListMeta
    {
        public List<Dummy> Dummy { get; }
        public List<StringText> StringText { get; }
        public List<SkillInfoData> SkillInfoData { get; }
    }


    [MessagePackObject(true)]
    public class Dummy : IDataKey<int>
    {
        public int Seed;
        public float Value;

        public Dummy(int seed, float value)
        {
            Seed = seed;
            Value = value;
        }

        [IgnoreMember]
        public int Key => Seed;
    }
    [MessagePackObject(true)]
    public class StringText : IDataKey<int>
    {
        public int Seed;
        public string Kor;
        public string Eng;

        public StringText(int seed, string kor, string eng)
        {
            Seed = seed;
            Kor = kor;
            Eng = eng;
        }

        [IgnoreMember]
        public int Key => Seed;
    }
    [MessagePackObject(true)]
    public class SkillInfoData : IDataKey<int>
    {
        public int Seed;
        public string Type;
        public string Tag;
        public int SkillGroup;
        public int NameIdx;
        public int DestIdx;
        public float CoolTIme;
        public string ActivateType;
        public float ActivateValue;
        public string TargetType;
        public int TargetValue;
        public string DamagePerType;
        public float DamagePerValue;
        public string EventNodePath;

        [IgnoreMember]
        public eSkillType type => Define.GetEnum<eSkillType>(Type);
        [IgnoreMember]
        public eSkillDetailType detailType => Define.GetEnum<eSkillDetailType>(Type);
        [IgnoreMember]
        public eSkillTag tag => Define.GetEnum<eSkillTag>(Tag);
        [IgnoreMember]
        public eSkillActivate activateType => Define.GetEnum<eSkillActivate>(ActivateType);
        [IgnoreMember]
        public eSkillTarget targetType => Define.GetEnum<eSkillTarget>(TargetType);
        [IgnoreMember]
        public eDamagePerType damagePerType => Define.GetEnum<eDamagePerType>(DamagePerType);

        public SkillInfoData(int seed, string type, string tag, int nameIdx, int destIdx, float coolTime, string activateType, float activateValue, 
            string targetType,int targetValue, string damagePerType, float damagePerValue, string eventNodePath)
        {
            Seed = seed;
            Type = type;
            Tag = tag;
            NameIdx = nameIdx;
            DestIdx = destIdx;
            CoolTIme = coolTime;
            ActivateType = activateType;
            ActivateValue = activateValue;
            TargetType = targetType;
            TargetValue = targetValue;
            DamagePerType = damagePerType;
            DamagePerValue = damagePerValue;
            EventNodePath = eventNodePath;
        }
        [IgnoreMember]
        public int Key => Seed;
    }


    public class UnitData : IDataKey<int>
    {
        public int Seed;
        public string Name;
        public eUnitType Type;

        public long hp;
        public long mp;
        public long atk;
        public long def;
        public float acc;
        public float atkSpd;
        public float moveSpd;
        public float atkRange;
        public float criRate;
        public float criDmg;
        public Texture2D icon;
        public string modelAssetRef;
        public string animatorAssetRef;
        public SkillInfo atkInfo;
        public List<SkillInfo> skillInfoGroup;

        public UnitData(Editor.UnitData data)
        {
            Seed = data.Seed;
            Name = data.Name;
            Type = data.Type;

            hp = data.info.hp;
            mp = data.info.mp;
            atk = data.info.atk;
            def = data.info.def;
            acc = data.info.acc;
            atkSpd = data.info.atkSpd;
            moveSpd = data.info.moveSpd;
            atkRange = data.info.atkRange;
            criRate = data.info.criRate;
            criDmg = data.info.criDmg;
            icon = data.info.icon;
            modelAssetRef = data.info.modelAssetRef;
            animatorAssetRef =data.info.animatorAssetRef;
            atkInfo = new SkillInfo(data.info.atkInfo);
            skillInfoGroup = new List<SkillInfo>();
            if(data.info.skillInfoGroup != null)
            {
                for (int i = 0; i < data.info.skillInfoGroup.Length; i++)
                {
                    skillInfoGroup.Add(new SkillInfo(data.info.skillInfoGroup[i]));
                }
            }
        }

        [IgnoreMember]
        public int Key => Seed;
    }
    public class SkillInfo
    {
        public int skillSeed;
        public eSkillActivate activateType;
        public float activateValue;
        public eSkillDuration durationType;
        public float durationValue;
        public eSkillTarget targetType;
        public float targetValue;
        public eSkillType skillType;
        public float typeValue;
        public float maxCoolTime;

        public string skillNode;

        private float coolTime;

        public SkillInfo(Editor.UnitData.SkillInfo skillInfo)
        {
            skillSeed = skillInfo.skillSeed;
            activateType = skillInfo.activateType;
            activateValue = skillInfo.activateValue;
            durationType = skillInfo.durationType;
            durationValue = skillInfo.durationValue;
            targetType = skillInfo.targetType;
            targetValue = skillInfo.targetValue;
            skillType = skillInfo.skillType;
            typeValue = skillInfo.typeValue;
            maxCoolTime = skillInfo.maxCoolTime;

            skillNode = skillInfo.skillNodeRef;
        }

        public void UpdateFrame(float deltaTime)
        {
            if (coolTime > 0)
                coolTime -= deltaTime;
        }

        public bool CheckCoolTime()
        {
            return coolTime > 0;
        }

        public void ResetSkill(float cool = 0)
        {
            if (cool == 0)
                coolTime = maxCoolTime;
            else
                coolTime = cool;
        }
    }
}