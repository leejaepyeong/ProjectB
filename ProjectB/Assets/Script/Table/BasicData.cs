using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Data
{
    public interface IDataKey<E>
    {
        public E Key { get; }
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
        public float dod;
        public float atkSpd;
        public float moveSpd;
        public float atkRange;
        public float criRate;
        public float criDmg;
        public long exp;
        public Texture2D icon;
        public string modelAssetRef;
        public string animatorAssetRef;
        public int atkIdx;
        public List<int> skillGroup = new List<int>();

        public SkillRecord atkInfo;
        public List<SkillRecord> skillInfoGroup = new List<SkillRecord>();

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
            dod = data.info.dod;
            atkSpd = data.info.atkSpd;
            moveSpd = data.info.moveSpd;
            atkRange = data.info.atkRange;
            criRate = data.info.criRate;
            criDmg = data.info.criDmg;
            exp = data.info.exp;
            icon = data.info.icon;
            modelAssetRef = data.info.modelAssetRef;
            animatorAssetRef =data.info.animatorAssetRef;
            atkInfo = TableManager.Instance.skillTable.GetRecord(data.info.atkInfoIdx);
            for (int i = 0; i < data.info.skillInfoIdxGroup.Length; i++)
            {
                skillInfoGroup.Add(TableManager.Instance.skillTable.GetRecord(data.info.skillInfoIdxGroup[i]));
            }
        }
        public int Key => Seed;
    }
}