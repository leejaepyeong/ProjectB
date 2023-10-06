using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class EquipSkill
{
    public SkillRecord skillRecord;
    public Dictionary<int, RuneRecord> runeDic = new Dictionary<int, RuneRecord>();

    public EquipSkill()
    {
        for (int i = 1; i <= Define.MaxEquipRune; i++)
        {
            runeDic.Add(i + 1, null);
        }
    }

    public void SetSkill(SkillRecord skill)
    {
        skillRecord = skill;
    }
    public void SetRune(RuneRecord rune, int slotIdx)
    {
        runeDic[slotIdx] = rune;
    }
    public bool CanEquipRune(RuneRecord rune)
    {
        if (skillRecord == null) return false;

        for (int i = 0; i < rune.runeTypeInfoList.Count; i++)
        {
            if (skillRecord.skillTags.Contains(rune.runeTypeInfoList[i].runeTag))
                return true;
        }
        return false;
    }
}
public class UISkillSlot_Inven : UISkillSlot
{
    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        UseSkill();
    }

    private void UseSkill()
    {

    }
}
