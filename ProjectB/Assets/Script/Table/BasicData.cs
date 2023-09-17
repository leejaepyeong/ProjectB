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
        public int SkillGroupSeed;
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
        [IgnoreMember]
        public EventGraph skillNode => BattleManager.Instance.ResourcePool.Load<EventGraph>(EventNodePath);

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
    }
    [MessagePackObject(true)]
    public class RuneInfoData : IDataKey<int>
    {
        public int Seed;
        public int GroupSeed;
        public int NameIdx;
        public int DestIdx;
        public string[] RuneType;
        public float[] RuneValue;
        public string[] RuneTag;

        public RuneInfoData(int seed, int groupSeed, int nameIdx, int destIdx, string[] runeType, float[] runeValues, string[] runeTag)
        {
            Seed = seed;
            GroupSeed = groupSeed;
            NameIdx = nameIdx;
            DestIdx = destIdx;
            RuneType = runeType;
            RuneValue = runeValues;
            RuneTag = runeTag;
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