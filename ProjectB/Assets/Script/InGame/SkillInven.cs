using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SkillInven : MonoBehaviour
{
    [FoldoutGroup("Info")] public List<UISkillSlot> skillSlots;
    [FoldoutGroup("Info")] private bool isPassive;

    public void Init()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            skillSlots[i].Init(i, isPassive);
            skillSlots[i].Open();
        }
    }
    public void Resetdata()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            skillSlots[i].ResetData();
        }
    }

    public void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            skillSlots[i].UpdateFrame(deltaTime);
        }
    }
}
