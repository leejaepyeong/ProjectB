using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

[Serializable]
public class UnitDataEditor_Data
{
    public enum eUnitType
    {
        Normal,
        Uniq,
        Boss,
    }


    public int Seed;
    public string Name;
    public eUnitType Type;
    public Info info = new();

    public UnitDataEditor_Data(int seed, string name)
    {
        Seed = seed;
        Name = name;
    }

    [Serializable]
    public class Info
    {
        [FoldoutGroup("Stat")]public int hp;
        [FoldoutGroup("Stat")] public int atk;
        [FoldoutGroup("Stat")] public float moveSpd;
        [VerticalGroup("Info")] public Texture2D icon;
        [VerticalGroup("Info")] public GameObject modelAssetRef;
        [VerticalGroup("Info")] public RuntimeAnimatorController animatorAssetRef;
    }
}
