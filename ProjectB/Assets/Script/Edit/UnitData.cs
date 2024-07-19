using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;


namespace Editor
{
    [Serializable]
    public class UnitData
    {
        [VerticalGroup("Basic/row"), LabelWidth(50)] public int Seed;
        [VerticalGroup("Basic/row"), LabelWidth(50)] public string Name;
        [VerticalGroup("Basic/row"), LabelWidth(50)] public eUnitType Type;
        [HideLabel, HorizontalGroup("Basic", 50), PreviewField(50, ObjectFieldAlignment.Right)]
        public Texture icon;
        public Info info = new();

        public UnitData(int seed, string name)
        {
            Seed = seed;
            Name = name;
        }

        [Serializable]
        public class Info
        {
            [FoldoutGroup("Stat")] public long hp;
            [FoldoutGroup("Stat")] public long mp;
            [FoldoutGroup("Stat")] public long atk;
            [FoldoutGroup("Stat")] public long def;
            [FoldoutGroup("Stat")] public float acc;
            [FoldoutGroup("Stat")] public float dod;
            [FoldoutGroup("Stat")] public float atkSpd;
            [FoldoutGroup("Stat")] public float moveSpd;
            [FoldoutGroup("Stat")] public float atkRange;
            [FoldoutGroup("Stat")] public float criRate;
            [FoldoutGroup("Stat")] public float criDmg;
            [VerticalGroup("Info")] public long exp;
            [VerticalGroup("Info")] public Texture2D icon;
            [VerticalGroup("Info")] public string modelAssetRef;
            [VerticalGroup("Info")] public string animatorAssetRef;
            [VerticalGroup("Info")] public int atkInfoIdx;
            [VerticalGroup("Info")] public int[] skillInfoIdxGroup;
            [VerticalGroup("immunity")] public bool isStunAble;
            [VerticalGroup("immunity")] public bool isSlowAble;
            [VerticalGroup("immunity")] public bool isDeathAble;

        }
    }



}