using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SkillInven : MonoBehaviour
{
    [FoldoutGroup("Info")] public List<UISkillSlot> skillSlots;

    public void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            skillSlots[i].UpdateFrame(deltaTime);
        }
    }
}
