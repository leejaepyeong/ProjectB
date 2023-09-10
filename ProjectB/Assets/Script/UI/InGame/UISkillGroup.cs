using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillGroup
{
    [SerializeField] private List<UISkillSlot> uiSkillSlots;

    public void Init()
    {
        for (int i = 0; i < uiSkillSlots.Count; i++)
        {
            uiSkillSlots[i].Init(i);
        }
    }

    public void UpdateFrame(float deltaTime)
    {

    }
}
