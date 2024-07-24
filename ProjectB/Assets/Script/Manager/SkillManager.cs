using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : BaseManager
{
    private List<SkillBehavior> skillList = new();
    public const string SKILLBEHAVIOR_ASSET_KEY = "Assets/Data/GameResources/Prefab/Behavior/SkillBehavior.prefab";

    private List<UnitBehavior> unitList = new List<UnitBehavior>();
    private List<UnitBehavior> tempUnitList = new List<UnitBehavior>();

    public static SkillManager Instance
    {
        get { return Manager.Instance.GetManager<SkillManager>(); }
    }

    public override void Init()
    {
        base.Init();
        skillList.Clear();
        unitList.Clear();
        tempUnitList.Clear();
    }

    public override void Clear()
    {
        RemoveAll();
        skillList.Clear();
        unitList.Clear();
        tempUnitList.Clear();
        base.Clear();
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (BattleManager.Instance.isPause) return;
        base.UpdateFrame(deltaTime);
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].isActiveAndEnabled == false) continue;
            skillList[i].UpdateFrame(DeltaTime, Time.timeScale);
        }
    }

    public SkillBehavior SpawnSkill(SkillInfo skillInfo, UnitBehavior caster, UnitBehavior target = null)
    {
        if (!GameObjectPool.TryGet(SKILLBEHAVIOR_ASSET_KEY, out var skillObj)) return null;
        SkillBehavior skill = skillObj.GetComponent<SkillBehavior>();

        skill.transform.SetParent(transform);
        skill.transform.position = caster.GetPos();
        skill.Init(caster);
        skill.Action(skillInfo.skillRecord.skillNode);
        skillList.Add(skill);

        return skill;
    }

    public void Remove(SkillBehavior skill)
    {
        skillList.Remove(skill);
        GameObjectPool.Return(skill.Model);
        GameObjectPool.Return(skill.gameObject);
    }

    public void RemoveAll()
    {
        while (skillList.Count > 0)
        {
            Remove(skillList[0]);
        }
    }

    public void UseSkill(UnitBehavior caster, SkillRecord skillRecord)
    {
        if (CheckSkill(caster, skillRecord) == false) return;
        skillRecord.SetCoolTime();

        var targetList = GetTargetList(caster, skillRecord);

        for (int i = 0; i < targetList.Count; i++)
        {
            ApplySkill(skillRecord, targetList[i], caster);
        }
    }

    public void UseSkillPlayer(UnitBehavior caster, SkillInfo skillInfo)
    {
        var targetList = GetTargetList(caster, skillInfo.skillRecord);

        for (int i = 0; i < targetList.Count; i++)
        {
            ApplySkill(skillInfo.skillRecord, targetList[i], caster);
        }
    }

    public void ApplySkill(SkillRecord skill, UnitBehavior target, UnitBehavior caster)
    {
        for (int i = 0; i < skill.skillEffects.Count; i++)
        {
            if (TableManager.Instance.skillEffectTable.TryGetRecord(skill.skillEffects[i], out var skillEffect) == false)
                continue;
            ApplySkillEffect(skillEffect, target, caster);
        }
    }

    public void ApplySkillEffect(SkillEffectRecord skillEffect, UnitBehavior target, UnitBehavior caster)
    {
        BuffBase buff = null;
        switch (skillEffect.skillState)
        {
            case eSkillState.Burn:
                buff = new BuffBase_Burn(skillEffect, caster, target);
                break;
            case eSkillState.Freeze:
                buff = new BuffBase_Frozen(skillEffect, caster, target);
                break;
            case eSkillState.Fear:
                buff = new BuffBase_Fear(skillEffect, caster, target);
                break;
            case eSkillState.Poison:
                buff = new BuffBase_Poison(skillEffect, caster, target);
                break;
            case eSkillState.Stun:
                buff = new BuffBase_Stun(skillEffect, caster, target);
                break;
            case eSkillState.AddStat:
                buff = new BuffBase_AddStat(skillEffect, caster, target);
                break;
        }
        target.UnitBase.AddBuff(buff);
    }

    public bool CheckSkill(UnitBehavior caster, SkillRecord skillRecord)
    {
        if (skillRecord.detailType == eSkillDetailType.Normal)
        {
            if (caster.UnitState.CanAction(eUnitFsm.skill))
            {
                caster.UnitState.SetFsm(eUnitFsm.skill);
                return true;
            }
            else
                return false;
        }
        else
        {
            caster.UnitState.SetFsm(eUnitFsm.skill_NonStop);
            return true;
        }
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
            case eSkillTarget.Random:
                unitList = GetTargetList_Random(caster, skillRecord, targetList);
                break;
            case eSkillTarget.Random_Overlap:
                unitList = GetTargetList_RandomOverlap(caster, skillRecord, targetList);
                break;
            case eSkillTarget.Team:
                break;
            case eSkillTarget.Self:
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
    private List<UnitBehavior> GetTargetList_Random(UnitBehavior caster, SkillRecord skillRecord, List<UnitBehavior> targetList = null)
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

        for (int i = 0; i < skillRecord.skillBulletTargetNum; i++)
        {
            if (list.Count < 1) break;
            int random = Random.Range(0, list.Count);
            tempUnitList.Add(list[random]);
            list.Remove(list[i]);
        }

        return tempUnitList;
    }
    private List<UnitBehavior> GetTargetList_RandomOverlap(UnitBehavior caster, SkillRecord skillRecord, List<UnitBehavior> targetList = null)
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

        for (int i = 0; i < skillRecord.skillBulletTargetNum; i++)
        {
            int random = Random.Range(0, list.Count);
            if (list.Count <= i) break;
            tempUnitList.Add(list[i]);
        }

        return tempUnitList;
    }
    #endregion
}
