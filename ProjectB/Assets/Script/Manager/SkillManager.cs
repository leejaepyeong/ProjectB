using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private List<UnitBehavior> unitList = new List<UnitBehavior>();
    private List<UnitBehavior> tempUnitList = new List<UnitBehavior>();

    public void UseSkill(UnitBehavior caster, SkillRecord skillRecord)
    {
        if (CheckSkill(caster, skillRecord) == false) return;
        skillRecord.SetCoolTime();


    }

    public void UseSkillPlayer(UnitBehavior caster, SkillInfo skillInfo)
    {
        var targetList = GetTargetList(caster, skillInfo.skillRecord);

        for (int i = 0; i < targetList.Count; i++)
        {
            ApplySkill(skillInfo.skillRecord);
            for (int j = 0; j < skillInfo.skillEffectList.Count; j++)
            {
                ApplySkillEffect(skillInfo.skillEffectList[j]);
            }
        }
    }

    private void ApplySkill(SkillRecord skill)
    {

    }

    private void ApplySkillEffect(SkillEffectRecord skillEffect)
    {

    }


    public bool CheckSkill(UnitBehavior caster, SkillRecord skillRecord)
    {
        return true;
    }

    #region Search Target List
    public List<UnitBehavior> GetTargetList(UnitBehavior caster, SkillRecord skillRecord, List<UnitBehavior> targetList = null)
    {
        unitList.Clear();
        tempUnitList.Clear();

        switch (skillRecord.targetType)
        {
            case eSkillTarget.Near:
                unitList = GetTargetList_Near(caster, skillRecord, targetList);
                break;
            case eSkillTarget.self:
                unitList.Add(caster);
                break;
        }

        return unitList;
    }

    private List<UnitBehavior> GetTargetList_Near(UnitBehavior caster, SkillRecord skillRecord, List<UnitBehavior> targetList = null)
    {
        List<UnitBehavior> list = new List<UnitBehavior>();

        if (targetList == null)
            list = UnitManager.Instance.UnitActiveList;
        else
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                list.Add(targetList[i]);
            }
        }

        list.Sort(delegate(UnitBehavior a, UnitBehavior b) 
        {
            float distanceA = Vector2.Distance(a.GetPos(), caster.GetPos());
            float distanceB = Vector2.Distance(b.GetPos(), caster.GetPos());

            return distanceA.CompareTo(distanceB);
        });

        for (int i = 0; i < skillRecord.skillBulletTargetNum; i++)
        {
            if (list.Count <= i) break;
            tempUnitList.Add(list[i]);
        }

        return tempUnitList;
    }

    #endregion
}
