using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base : MonoBehaviour
{
    protected UnitBehavior unitBehavior;
    protected UnitState unitState;
    protected Data.UnitData unitData;

    protected Transform target;
    protected float deltaTime;

    protected long curHp;
    protected long curMp;

    protected bool isAtkAble;
    protected float atkCool;

    public virtual void Init(UnitBehavior behavior)
    {
        unitBehavior = behavior;
        unitState = unitBehavior.UnitState;
        unitData = unitBehavior.UnitData;
        atkCool = 1 / unitState.atkSpd;
        isAtkAble = true;
        curHp = unitState.hp;
        curMp = unitState.mp;
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        if (unitState.isDead) return;

        this.deltaTime = deltaTime;
        if (unitState.isMoveAble == false) return;

        Move();
        SearchTarget();
        SkillUpdate();
    }

    public virtual void Move()
    {
        
    }

    public virtual void SearchTarget()
    {
        if (unitBehavior.isUseSkill) return;
        if(isAtkAble == false)
        {
            if (atkCool > 0)
            {
                atkCool -= deltaTime;
                return;
            }
            isAtkAble = true;
        }

        int layer = unitState.team == eTeam.player ? LayerMask.GetMask("Monster") : LayerMask.GetMask("Player");
        var targets = Physics2D.OverlapCircleAll(unitBehavior.GetPos(), unitState.atkRange, layer);
        if (targets.Length <= 0) return;

        atkCool = 1 / unitState.atkSpd;
        Attack();
    }

    public virtual void Attack()
    {
        isAtkAble = false;
        var targets = Manager.Instance.skillManager.GetTargetList(unitBehavior, unitData.atkInfo);
        unitBehavior.SetTargets(targets);
        unitBehavior.Action(unitData.atkInfo.skillNode);
    }

    public virtual void SkillUpdate()
    {
        for (int i = 0; i < unitData.skillInfoGroup.Count; i++)
        {
            var skill = unitData.skillInfoGroup[i];
            Manager.Instance.skillManager.UseSkill(unitBehavior, unitData.skillInfoGroup[i]);
            unitData.skillInfoGroup[i].UpdateFrame(deltaTime);
            if (skill.type == eSkillType.Passive && skill.IsReadyCoolTime())
                Manager.Instance.skillManager.UseSkill(unitBehavior, skill);
        }
    }

    public void ApplyDamage(float dmgPercent, eDamageType dmgType = eDamageType.Normal)
    {
        switch (dmgType)
        {
            case eDamageType.Normal:
            default:
                curHp -= (long)(unitState.atk * dmgPercent);
                break;
            case eDamageType.PerHp:
                curHp -= (long)(curHp * dmgPercent);
                break;
            case eDamageType.PerMaxHp:
                curHp -= (long)(unitState.hp * dmgPercent);
                break;
        }

        if (curHp <= 0)
        {
            unitState.isDead = true;
            UnitManager.Instance.RemoveUnit(unitBehavior);
        }
    }
}
