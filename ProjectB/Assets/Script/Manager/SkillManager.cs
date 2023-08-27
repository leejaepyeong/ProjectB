using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private List<UnitBehavior> unitList = new List<UnitBehavior>();

    public void UseSkill(UnitBehavior caster, Data.SkillInfo skillInfo)
    {

    }

    public bool CheckSkill(UnitBehavior caster, Data.SkillInfo skillInfo)
    {
        if (skillInfo.CheckCoolTime()) return false;

        switch (skillInfo.activateType)
        {
            case eSkillActivate.hitRate:
                break;
            case eSkillActivate.passive:
                return true;
        }

        return false;
    }

    #region Search Target List
    public List<UnitBehavior> GetTargetList(UnitBehavior caster, Data.SkillInfo skillInfo)
    {
        unitList.Clear();

        switch (skillInfo.targetType)
        {
            case eSkillTarget.normal:
                break;
            case eSkillTarget.self:
                unitList.Add(caster);
                break;
        }

        return unitList;
    }

    private List<UnitBehavior> GetTargetList_Normal()
    {
        return unitList;
    }

    #endregion
}
