using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessagePack;
using UnityEngine;

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
        public Define.eUnitType Type;

        public long hp;
        public long atk;
        public long def;
        public float atkSpd;
        public float moveSpd;
        public float atkRange;
        public Texture2D icon;
        public string modelAssetRef;
        public string animatorAssetRef;
        public string atkEventNodePath;
        public string skillEventNodePath;

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
            modelAssetRef = data.info.modelAssetPath;
            animatorAssetRef = data.info.animatorAssetPath;
            atkEventNodePath = data.info.atkEventNodePath;
            skillEventNodePath = data.info.skillEventNodePath;
        }

        [IgnoreMember]
        public int Key => Seed;
    }
}