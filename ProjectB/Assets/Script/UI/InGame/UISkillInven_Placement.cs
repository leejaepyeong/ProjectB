using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillInven_Placement : UISkillInven
{
    public UIInvenItemSlot selectSlot;
    public UISkillInven uiSkillInven;

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < mainSkillGroup.uiSkillSlots.Count; i++)
        {
            mainSkillGroup.uiSkillSlots[i].actionClose += Close;
        }
        for (int i = 0; i < activeSkillInven.skillSlots.Count; i++)
        {
            activeSkillInven.skillSlots[i].actionClose += Close;
        }
        for (int i = 0; i < passiveSkillInven.skillSlots.Count; i++)
        {
            passiveSkillInven.skillSlots[i].actionClose += Close;
        }
    }
    public virtual void Open(UIInvenItemSlot invenSlot)
    {
        BattleManager.Instance.isPause = true;
        selectSlot = invenSlot;
        base.Open();
    }

    public override void Close()
    {
        BattleManager.Instance.isPause = false;
        base.Close();
    }

    public override void ResetData()
    {
        base.ResetData();

        mainSkillGroup.gameObject.SetActive(selectSlot.itemType == eItemType.Rune);
        activeSkillInven.gameObject.SetActive(selectSlot.itemType == eItemType.Skill && selectSlot.getSkillRecord.type != eSkillType.Passive);
        passiveSkillInven.gameObject.SetActive(selectSlot.itemType == eItemType.Skill && selectSlot.getSkillRecord.type == eSkillType.Passive);

        if(selectSlot.itemType == eItemType.Rune)
        {
            for (int i = 0; i < mainSkillGroup.uiSkillSlots.Count; i++)
            {
                mainSkillGroup.uiSkillSlots[i].Open();
            }
        }
        else
        {
            for (int i = 0; i < activeSkillInven.skillSlots.Count; i++)
            {
                activeSkillInven.skillSlots[i].Open();
            }
            for (int i = 0; i < passiveSkillInven.skillSlots.Count; i++)
            {
                passiveSkillInven.skillSlots[i].Open();
            }
        }
    }
}
