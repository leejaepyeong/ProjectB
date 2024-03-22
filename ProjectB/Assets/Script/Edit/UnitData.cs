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
        public int Seed;
        public string Name;
        public eUnitType Type;
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
            [VerticalGroup("Info")] public Texture2D icon;
            [VerticalGroup("Info")] public string modelAssetRef;
            [VerticalGroup("Info")] public string animatorAssetRef;
            [VerticalGroup("Info")] public int atkInfoIdx;
            [VerticalGroup("Info")] public int[] skillInfoIdxGroup;
        }
    }



}