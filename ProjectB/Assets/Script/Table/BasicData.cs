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
        public int SkillGroupSeed;
        public int NameIdx;
        public int DestIdx;
        public string Type;
        public string DetailType;

        public string ActivateType;
        public float ActivateValue;

        public float CoolTIme;
        public string TargetType;
        public int TargetValue;
        public string DamagePerType;
        public float DamagePerValue;
        public int EquipRuneCount;
        public int SkillTag1;
        public int SkillTag2;
        public int SkillTag3;
        public int SkillTag4;
        public int SkillTag5;
        public string EventNodePath;

        [IgnoreMember]
        public eSkillType type => Define.GetEnum<eSkillType>(Type);
        [IgnoreMember]
        public eSkillDetailType detailType => Define.GetEnum<eSkillDetailType>(DetailType);
        [IgnoreMember]
        public eSkillActivate activateType => Define.GetEnum<eSkillActivate>(ActivateType);
        [IgnoreMember]
        public eSkillTarget targetType => Define.GetEnum<eSkillTarget>(TargetType);
        [IgnoreMember]
        public eDamagePerType damagePerType => Define.GetEnum<eDamagePerType>(DamagePerType);
        [IgnoreMember]
        public EventGraph skillNode => BattleManager.Instance.ResourcePool.Load<EventGraph>(EventNodePath);

        public SkillInfoData(int seed, int skillGroupSeed, string type, string detailType, int nameIdx, int destIdx, float coolTime, 
            string targetType, string damagePerType, float damagePerValue,int equipRuneCount, int skillTag1, int skillTag2, int skillTag3, int skillTag4, int skillTag5, string eventNodePath)
        {
            Seed = seed;
            SkillGroupSeed = skillGroupSeed;
            Type = type;
            DetailType = detailType;
            NameIdx = nameIdx;
            DestIdx = destIdx;
            CoolTIme = coolTime;
            TargetType = targetType;
            DamagePerType = damagePerType;
            DamagePerValue = damagePerValue;
            EquipRuneCount = equipRuneCount;
            SkillTag1 = skillTag1;
            SkillTag2 = skillTag2;
            SkillTag3 = skillTag3;
            SkillTag4 = skillTag4;
            SkillTag5 = skillTag5;
            EventNodePath = eventNodePath;
        }
        [IgnoreMember]
        public int Key => Seed;

        [IgnoreMember]
        private float elaspedTime;
        public void UpdateFrame(float deltaTime)
        {
            if (elaspedTime <= 0) return;
            elaspedTime -= deltaTime;
        }
        public void SetCoolTime(float time = 0)
        {
            if (time == 0) elaspedTime = CoolTIme;
            else elaspedTime = time;
        }
        public bool IsReadyCoolTime()
        {
            return elaspedTime <= 0;
        }
        private List<int> skilLTags;
        [IgnoreMember]
        public List<int> SkillTags
        {
            get
            {
                if(skilLTags == null)
                {
                    skilLTags = new List<int>() { };
                    skilLTags.Add(SkillTag1);
                    skilLTags.Add(SkillTag2);
                    skilLTags.Add(SkillTag3);
                    skilLTags.Add(SkillTag4);
                    skilLTags.Add(SkillTag5);
                }
                return skilLTags;
            }
        }
    }
    [MessagePackObject(true)]
    public class RuneInfoData : IDataKey<int>
    {
        public class RuneTypeInfo
        {
            public eRuneType runeType;
            public float value;

            public RuneTypeInfo(eRuneType runeType, float value)
            {
                this.runeType = runeType;
                this.value = value;
            }
        }

        public int Seed;
        public int GroupSeed;
        public int NameIdx;
        public int DestIdx;
        public string RuneType1;
        public float RuneValue1;
        public string RuneType2;
        public float RuneValue2;
        public string RuneType3;
        public float RuneValue3;
        public string RuneType4;
        public float RuneValue4;
        public int RuneTag1;
        public int RuneTag2;
        public int RuneTag3;
        public int RuneTag4;
        public int RuneTag5;

        public RuneInfoData(int seed, int groupSeed, int nameIdx, int destIdx, string runeType1, string runeType2, string runeType3, string runeType4, 
            float runeValue1, float runeValue2, float runeValue3, float runeValue4, int runeTag1, int runeTag2, int runeTag3, int runeTag4, int runeTag5)
        {
            Seed = seed;
            GroupSeed = groupSeed;
            NameIdx = nameIdx;
            DestIdx = destIdx;
            RuneType1 = runeType1;
            RuneType2 = runeType2;
            RuneType3 = runeType3;
            RuneType4 = runeType4;
            RuneValue1 = runeValue1;
            RuneValue2 = runeValue2;
            RuneValue3 = runeValue3;
            RuneValue4 = runeValue4;
            RuneTag1 = runeTag1;
            RuneTag2 = runeTag2;
            RuneTag3 = runeTag3;
            RuneTag4 = runeTag4;
            RuneTag5 = runeTag5;
        }
        [IgnoreMember]
        public int Key => Seed;

        private List<RuneTypeInfo> runeTypeInfoList;
        [IgnoreMember]
        public List<RuneTypeInfo> RuneTypeInfoList
        {
            get
            {
                if(runeTypeInfoList == null)
                {
                    runeTypeInfoList = new List<RuneTypeInfo>();
                    runeTypeInfoList.Add(new RuneTypeInfo(Define.GetEnum<eRuneType>(RuneType1) ,RuneValue1));
                    runeTypeInfoList.Add(new RuneTypeInfo(Define.GetEnum<eRuneType>(RuneType2), RuneValue2));
                    runeTypeInfoList.Add(new RuneTypeInfo(Define.GetEnum<eRuneType>(RuneType3), RuneValue3));
                    runeTypeInfoList.Add(new RuneTypeInfo(Define.GetEnum<eRuneType>(RuneType4), RuneValue4));
                }

                return runeTypeInfoList;
            }
        }
        private List<int> runeTags;
        [IgnoreMember]
        public List<int> RuneTags
        {
            get
            {
                if(runeTags == null)
                {
                    runeTags = new List<int>();
                    runeTags.Add(RuneTag1);
                    runeTags.Add(RuneTag2);
                    runeTags.Add(RuneTag3);
                    runeTags.Add(RuneTag4);
                    runeTags.Add(RuneTag5);
                }
                return runeTags;
            }
        }
    }
    [MessagePackObject(true)]
    public class SkillEffectInfo : IDataKey<int>
    {
        public int Seed;
        public int GroupSeed;
        public int NameIdx;
        public int DestIdx;
        public string SkillState;
        public string BuffType;
        public float SkillValue;
        public float SkillDuration;

        [IgnoreMember]
        public eSkillState skillState => Define.GetEnum<eSkillState>(SkillState);
        [IgnoreMember]
        public eBuffType buffType => Define.GetEnum<eBuffType>(BuffType);

        public SkillEffectInfo(int seed, int groupSeed, int nameIdx, int destIdx, string skillState, string buffType, float skillValue, float skillDuration)
        {
            Seed = seed;
            GroupSeed = groupSeed;
            NameIdx = nameIdx;
            DestIdx = destIdx;
            SkillState = skillState;
            BuffType = buffType;
            SkillValue = skillValue;
            SkillDuration = skillDuration;
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
        public Data.SkillInfoData atkInfo;
        public List<Data.SkillInfoData> skillInfoGroup;

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
            skillInfoGroup = new List<Data.SkillInfoData>();
        }

        [IgnoreMember]
        public int Key => Seed;
    }
}