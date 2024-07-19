using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;
using System.Security.Cryptography;

namespace Editor
{
    [Serializable]
    public class SkillData
    {
        [HorizontalGroup("Info", 180, 10), LabelText("스테이지 인덱스"), LabelWidth(100)] public int Seed;
        [HorizontalGroup("Info", 160, 5), LabelWidth(50)] public string Name;
        [HorizontalGroup("Info", 180, 5), LabelText("스테이지 타입"), LabelWidth(80)] public eSkillType skillType;

        public SkillData(int seed, string name)
        {
            Seed = seed;
            Name = name;
        }

        public ACondition aCondition;


    }
}
