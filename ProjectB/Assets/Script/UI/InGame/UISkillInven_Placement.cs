using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillInven_Placement : UISkillInven
{
    public UIInvenItemSlot selectSlot;
    private UISkillInven uiSkillInven;

    public override void Init()
    {
        for (int i = 0; i < mainSkillGroup.uiSkillSlots.Count; i++)
        {
            mainSkillGroup.uiSkillSlots[i] = uiSkillInven.mainSkillGroup.uiSkillSlots[i];
        }
        Close();
    }
    public virtual void Open(UIInvenItemSlot invenSlot)
    {
        base.Open();
        selectSlot = invenSlot;
        ResetData();
    }

    public override void ResetData()
    {
        mainSkillGroup.gameObject.SetActive(selectSlot.isRune);
        activeSkillInven.gameObject.SetActive(selectSlot.isRune == false);
        passiveSkillInven.gameObject.SetActive(selectSlot.isRune == false);

        if(selectSlot.isRune)
        {
            for (int i = 0; i < mainSkillGroup.uiSkillSlots.Count; i++)
            {
                mainSkillGroup.uiSkillSlots[i].Open(this);
            }
        }
        else
        {
            for (int i = 0; i < activeSkillInven.skillSlots.Count; i++)
            {
                activeSkillInven.skillSlots[i].Open(this);
            }
            for (int i = 0; i < passiveSkillInven.skillSlots.Count; i++)
            {
                passiveSkillInven.skillSlots[i].Open(this);
            }
        }
    }
}
