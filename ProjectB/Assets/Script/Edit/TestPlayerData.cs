using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TestPlayerData", menuName = "")]
public class TestPlayerData : ScriptableObject
{
    [FoldoutGroup("Stat")] public long hp;
    [FoldoutGroup("Stat")] public long mp;
    [FoldoutGroup("Stat")] public long atk;
    [FoldoutGroup("Stat")] public long def;
    [FoldoutGroup("Stat")] public float acc;
    [FoldoutGroup("Stat")] public float dod;
    [FoldoutGroup("Stat")] public float moveSpd;
    [FoldoutGroup("Stat")] public float atkSpd;
    [FoldoutGroup("Stat")] public float atkRange;
    [FoldoutGroup("Stat")] public float criRate;
    [FoldoutGroup("Stat")] public float criDmg;

    [FoldoutGroup("Skill")] public List<int> equipSkill;
}
