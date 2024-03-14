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

    protected List<BuffBase> buffList = new List<BuffBase>();
    protected List<UnitBehavior> targetList = new List<UnitBehavior>();

    public virtual void Init(UnitBehavior behavior)
    {
        unitBehavior = behavior;
        unitState = unitBehavior.UnitState;
        unitData = unitBehavior.UnitData;
        atkCool = 1 / (float)unitState.GetStat(eStat.atkSpd);
        isAtkAble = true;
        curHp = (long)unitState.GetStat(eStat.hp);
        curMp = (long)unitState.GetStat(eStat.mp);
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        if (unitState.isDead) return;

        this.deltaTime = deltaTime;
        if (unitState.isMoveAble == false) return;

        Move();
        SearchTarget();
        SkillUpdate();

        for (int i = 0; i < buffList.Count; i++)
        {
            buffList[i].UpdateFrame(deltaTime);
            if (buffList[i].CheckEndBuff())
                RemoveBuff(buffList[i]);
        }
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

        int layer = unitState.team == eTeam.player ? LayerMask.GetMask("Monster") : LayerMask.GetMask("Player");
        var targets = Physics2D.OverlapCircleAll(unitBehavior.GetPos(), (float)unitState.GetStat(eStat.atkRange), layer);
        if (targets.Length <= 0) return;
        targetList.Clear();
        for (int i = 0; i < targets.Length; i++)
        {
            UnitBehavior target = targets[i].GetComponentInParent<UnitBehavior>();
            if (target != null)
                targetList.Add(target);
        }

        atkCool = 1 / (float)unitState.GetStat(eStat.atkSpd);
        Attack();
    }

    public virtual void Attack()
    {
        isAtkAble = false;
        unitBehavior.Action(unitData.atkInfo.skillNode);
    }

    public virtual void SkillUpdate()
    {
        for (int i = 0; i < unitData.skillInfoGroup.Count; i++)
        {
            var skill = unitData.skillInfoGroup[i];
            unitData.skillInfoGroup[i].UpdateFrame(deltaTime);
            if (skill.type == eSkillType.Passive && skill.IsReadyCoolTime())
                Manager.Instance.skillManager.UseSkill(unitBehavior, skill);
        }
    }

    public void ApplyDamage(UnitBehavior caster ,float dmgPercent, eDamagePerType dmgType = eDamagePerType.Atk)
    {
        switch (dmgType)
        {
            case eDamagePerType.Atk:
            default:
                curHp -= (long)(caster.UnitState.GetStat(eStat.atk) * dmgPercent);
                break;
            case eDamagePerType.Def:
                curHp -= (long)(caster.UnitState.GetStat(eStat.def) * dmgPercent);
                break;
            case eDamagePerType.AtkSpd:
                curHp -= (long)(curHp * dmgPercent);
                break;
            case eDamagePerType.CurHp:
                curHp -= (long)(caster.UnitState.GetStat(eStat.atk) * dmgPercent);
                break;
            case eDamagePerType.MaxHp:
                curHp -= (long)(caster.UnitState.GetStat(eStat.hp) * dmgPercent);
                break;
        }

        if (curHp <= 0)
        {
            unitState.isDead = true;
            UnitManager.Instance.RemoveUnit(unitBehavior);
        }
    }

    #region Buff
    public void RemoveBuff(BuffBase buff)
    {
        buffList.Remove(buff);
    }
    #endregion
}
