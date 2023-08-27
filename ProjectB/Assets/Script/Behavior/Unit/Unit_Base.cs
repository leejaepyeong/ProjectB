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

    protected bool isAtkAble;
    protected float atkCool;

    public virtual void Init(UnitBehavior behavior)
    {
        unitBehavior = behavior;
        unitState = unitBehavior.UnitState;
        unitData = unitBehavior.UnitData;
        atkCool = unitState.atkSpd;
        isAtkAble = true;
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        if (unitState.isDead) return;

        this.deltaTime = deltaTime;
        if (unitState.isMoveAble == false) return;

        Move();
        SearchTarget();
        PassiveSkill();
    }

    public virtual void Move()
    {
        
    }

    public virtual void SearchTarget()
    {
        if(isAtkAble == false)
        {
            if (atkCool > 0)
            {
                atkCool -= deltaTime;
                return;
            }
            isAtkAble = true;
        }

        var targets = Physics2D.OverlapCircleAll(unitBehavior.GetPos(), unitState.atkRange);
        if (targets.Length <= 0) return;

        atkCool = unitState.atkSpd;
        Attack();
    }

    public virtual void Attack()
    {
        var targets = Manager.Instance.skillManager.GetTargetList(unitBehavior, unitData.atkInfo);
        unitBehavior.SetTargets(targets);
        unitBehavior.Action(unitData.atkInfo.skillNode);
    }

    public virtual void PassiveSkill()
    {
        for (int i = 0; i < unitData.skillInfoGroup.Count; i++)
        {
            Manager.Instance.skillManager.UseSkill(unitBehavior, unitData.skillInfoGroup[i]);
            unitData.skillInfoGroup[i].UpdateFrame(deltaTime);
        }
    }
}
