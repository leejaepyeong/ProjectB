using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillGroup : MonoBehaviour
{
    public List<UISkillSlot> uiSkillSlots;

    public void Init()
    {
        for (int i = 0; i < uiSkillSlots.Count; i++)
        {
            uiSkillSlots[i].Init(i);
            uiSkillSlots[i].Open();
        }
    }

    public void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < uiSkillSlots.Count; i++)
        {
            uiSkillSlots[i].UpdateFrame(deltaTime);
        }
    }
}
