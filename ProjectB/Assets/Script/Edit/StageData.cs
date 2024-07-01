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
    public class StageData
    {
        public int Seed;
        public string Name;
        public eStageType Type;
        public Info info = new();

        public StageData(int seed, string name)
        {
            Seed = seed;
            Name = name;
        }

        [Serializable]
        public class Info
        {
            [FoldoutGroup("Stat")] public long hp;
            [VerticalGroup("Info")] public long exp;
            [VerticalGroup("Spawn")] public SpawnInfo[] spawnInfoes;
        }

        [Serializable]
        public class SpawnInfo
        {
            [VerticalGroup("Info")] public int monsterSeed;
            [VerticalGroup("Info")] public int monsterLevel;
            [VerticalGroup("Info")] public float startDelay;
            [VerticalGroup("Info")] public float coolTime;
            [VerticalGroup("Info")] public bool isBuff;

            [ShowIf("@isBuff == true"), VerticalGroup("Info"), FoldoutGroup("Buff")] public eBuffType startBuffType;
            [ShowIf("@isBuff == true"), VerticalGroup("Info"), FoldoutGroup("Buff")] public eStat startBuff;
            [ShowIf("@isBuff == true"), VerticalGroup("Info"), FoldoutGroup("Buff")] public eSkillDuration durationType;

        }
    }



}