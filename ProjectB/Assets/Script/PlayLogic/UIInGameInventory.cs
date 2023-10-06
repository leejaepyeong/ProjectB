using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIInGameInventory : UIBase
{
    public List<SkillRecord> skillList;
    public List<RuneRecord> runeList;

    public Dictionary<int, EquipSkill> equipSkillDic = new Dictionary<int, EquipSkill>();
    public List<EquipSkill> equipSkillList = new List<EquipSkill>();

    public override void Init()
    {
        skillList.Clear();
        runeList.Clear();
        equipSkillList.Clear();
        for (int i = 1; i <= Define.MaxEquipSkill; i++)
        {
            equipSkillDic.Add(i, new EquipSkill());
        }
    }

    public void EquipSkill(SkillRecord skill, int slotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;

        equipSkill.SetSkill(skill);
    }

    public void EquipRune(int slotIdx, RuneRecord rune, int runeSlotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;
        if (equipSkill.CanEquipRune(rune) == false) return;

        equipSkill.SetRune(rune, runeSlotIdx);
    }

    public void UnEquipSkill(int slotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;
        if (equipSkill.skillRecord == null) return;

        equipSkill.skillRecord = null;
    }

    public void UnEquipRune(int slotIdx, int runeSlotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;
        if (equipSkill.skillRecord == null) return;

        equipSkill.runeDic[runeSlotIdx] = null;
    }
}
