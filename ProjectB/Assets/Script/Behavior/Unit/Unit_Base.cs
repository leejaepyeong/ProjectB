using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base : MonoBehaviour
{
    private UnitBehavior unitBehavior;
    private UnitState unitState;
    private Data.UnitData unitData;

    public virtual void Init(UnitBehavior behavior)
    {
        unitBehavior = behavior;
        unitState = unitBehavior.UnitState;
        unitData = unitBehavior.UnitData;
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        if (unitState.isDead) return;

        Move();
        SearchTarget();
        Skill();
    }

    public virtual void Move()
    {
    }

    public virtual void SearchTarget()
    {
    }

    public virtual void Attack()
    {
        unitBehavior.Action("","");
    }

    public virtual void Skill()
    {

    }
}
