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
        public Define.eUnitType Type;
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
            [FoldoutGroup("Stat")] public long atk;
            [FoldoutGroup("Stat")] public long def;
            [FoldoutGroup("Stat")] public float atkSpd;
            [FoldoutGroup("Stat")] public float moveSpd;
            [FoldoutGroup("Stat")] public float atkRange;
            [VerticalGroup("Info")] public Texture2D icon;
            [VerticalGroup("Info")] public GameObject modelAssetRef;
            [VerticalGroup("Info"), ReadOnly] public string modelAssetPath => AssetDatabase.GetAssetPath(modelAssetRef);
            [VerticalGroup("Info")] public RuntimeAnimatorController animatorAssetRef;
            [VerticalGroup("Info"), ReadOnly] public string animatorAssetPath => AssetDatabase.GetAssetPath(animatorAssetRef);
            public AssetReference test;
            [VerticalGroup("Info")] public EventGraph atkEventNode;
            [VerticalGroup("Info"), ReadOnly] public string atkEventNodePath => AssetDatabase.GetAssetPath(atkEventNode);
            [VerticalGroup("Info")] public EventGraph skillEventNode;
            [VerticalGroup("Info"), ReadOnly] public string skillEventNodePath => AssetDatabase.GetAssetPath(skillEventNode);
        }
    }

}