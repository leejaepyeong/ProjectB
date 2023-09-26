using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private List<UnitBehavior> unitList = new List<UnitBehavior>();
    private List<UnitBehavior> tempUnitList = new List<UnitBehavior>();

    public void UseSkill(UnitBehavior caster, Data.SkillInfoData skillInfo)
    {
        if (CheckSkill(caster, skillInfo) == false) return;
        skillInfo.SetCoolTime();


    }

    public void UseSkill(UnitBehavior caster, SkillInfo skillInfo)
    {
    }

    private void ApplySkill()
    {

    }

    private void ApplyBuff()
    {

    }


    public bool CheckSkill(UnitBehavior caster, Data.SkillInfoData skillInfo)
    {
        return true;
    }

    #region Search Target List
    public List<UnitBehavior> GetTargetList(UnitBehavior caster, Data.SkillInfoData skillInfo)
    {
        unitList.Clear();
        tempUnitList.Clear();

        switch (skillInfo.targetType)
        {
            case eSkillTarget.normal:
                unitList = GetTargetList_Normal(caster, skillInfo);
                break;
            case eSkillTarget.self:
                unitList.Add(caster);
                break;
        }

        return unitList;
    }

    private List<UnitBehavior> GetTargetList_Normal(UnitBehavior caster, Data.SkillInfoData skillInfo)
    {
        var list = UnitManager.Instance.UnitActiveList;

        list.Sort(delegate(UnitBehavior a, UnitBehavior b) 
        {
            float distanceA = Vector2.Distance(a.GetPos(), caster.GetPos());
            float distanceB = Vector2.Distance(b.GetPos(), caster.GetPos());

            return distanceA.CompareTo(distanceB);
        });

        for (int i = 0; i < skillInfo.SkillBulletTargetNum; i++)
        {
            if (list.Count <= i) break;
            tempUnitList.Add(list[i]);
        }

        return tempUnitList;
    }

    #endregion
}
