using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UIRuneChangeDlg : UIDlg
{
    [SerializeField, FoldoutGroup("Info")] private List<UIRuneSlot> uiRuneSlotGroup;

    private UISkillSlot uiSkillSlot;
    private UIInvenItemSlot invenSlot;

    public virtual void Open(UISkillSlot skillSlot, UIInvenItemSlot invenSlot)
    {
        base.Open();
        uiSkillSlot = skillSlot;
        this.invenSlot = invenSlot;
        ResetData();
    }

    public override void ResetData()
    {
        for (int i = 0; i < uiRuneSlotGroup.Count; i++)
        {
            uiRuneSlotGroup[i].Open(uiSkillSlot.SkillInfo.runeDic[i], EquipRune);
        }
    }

    private void EquipRune(int slotIdx)
    {
        if(uiSkillSlot.SkillInfo.runeDic[slotIdx] != null)
        {

        }
        else
        {

        }
    }
}
