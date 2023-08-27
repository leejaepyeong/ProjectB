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
    }
    [MessagePackObject(true)]
    public class ListMeta
    {
        public List<Dummy> Dummy { get; }
        public List<StringText> StringText { get; }
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

    public class UnitData : IDataKey<int>
    {
        public int Seed;
        public string Name;
        public eUnitType Type;

        public long hp;
        public long atk;
        public long def;
        public float atkSpd;
        public float moveSpd;
        public float atkRange;
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
            atk = data.info.atk;
            def = data.info.def;
            atkSpd = data.info.atkSpd;
            moveSpd = data.info.moveSpd;
            atkRange = data.info.atkRange;
            icon = data.info.icon;
            modelAssetRef = AssetDatabase.GetAssetPath(data.info.modelAssetRef);
            animatorAssetRef = AssetDatabase.GetAssetPath(data.info.animatorAssetRef);
            atkInfo = new SkillInfo(data.info.atkInfo);
            skillInfoGroup = new List<SkillInfo>();
            for (int i = 0; i < data.info.skillInfoGroup.Length; i++)
            {
                skillInfoGroup.Add(new SkillInfo(data.info.skillInfoGroup[i]));
            }
        }

        [IgnoreMember]
        public int Key => Seed;
    }
    public class SkillInfo
    {
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
            activateType = skillInfo.activateType;
            activateValue = skillInfo.activateValue;
            durationType = skillInfo.durationType;
            durationValue = skillInfo.durationValue;
            targetType = skillInfo.targetType;
            targetValue = skillInfo.targetValue;
            skillType = skillInfo.skillType;
            typeValue = skillInfo.typeValue;
            maxCoolTime = skillInfo.maxCoolTime;

            skillNode = AssetDatabase.GetAssetPath(skillInfo.skillNode);
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