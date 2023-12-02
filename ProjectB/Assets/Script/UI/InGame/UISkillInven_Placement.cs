using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UISkillInven_Placement : UIBase
{
    [SerializeField, FoldoutGroup("Info")] UISkillSlot[] mainSkillGroup;
    [SerializeField, FoldoutGroup("Info")] UISkillSlot[] activeSkillGroup;
    [SerializeField, FoldoutGroup("Info")] UISkillSlot[] passiveGroup;

    private UIInvenItemSlot selectSlot;

    public virtual void Open(UIInvenItemSlot invenSlot)
    {
        base.Open();
        selectSlot = invenSlot;
        ResetData();
    }

    public override void ResetData()
    {
        
    }
}
