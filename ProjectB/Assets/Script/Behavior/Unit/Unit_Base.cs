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

    public long curHp;
    public long curMp;

    protected bool isAtkAble;
    protected float atkCool;

    protected Dictionary<int, BuffBase> buffDic = new Dictionary<int, BuffBase>();
    protected Dictionary<eStat, List<BuffBase>> buffStatDic = new Dictionary<eStat, List<BuffBase>>();
    protected List<UnitBehavior> targetList = new List<UnitBehavior>();

    public virtual void Init(UnitBehavior behavior)
    {
        unitBehavior = behavior;
        unitState = unitBehavior.UnitState;
        unitData = unitBehavior.UnitData;
        atkCool = 1 / (float)GetStat(eStat.atkSpd);
        isAtkAble = true;
        curHp = (long)GetStat(eStat.hp);
        curMp = (long)GetStat(eStat.mp);
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        if (unitState.isDead) return;

        this.deltaTime = deltaTime;
        if (unitState.isMoveAble == false) return;

        Move();
        SearchTarget();
        SkillUpdate();

        foreach (var buff in buffDic)
        {
            buff.Value.UpdateFrame(deltaTime);
            if (buff.Value.CheckEndBuff())
                RemoveBuff(buff.Value);
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
        var targets = Physics2D.OverlapCircleAll(unitBehavior.GetPos(), (float)GetStat(eStat.atkRange), layer);
        if (targets.Length <= 0) return;
        targetList.Clear();
        for (int i = 0; i < targets.Length; i++)
        {
            UnitBehavior target = targets[i].GetComponentInParent<UnitBehavior>();
            if (target != null)
                targetList.Add(target);
        }

        atkCool = 1 / (float)GetStat(eStat.atkSpd);
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

    public double GetStat(eStat statType)
    {
        double value = unitState.originStatValue[statType];

        #region CheckBuff
        #endregion

        #region Rune
        if(unitState.team == eTeam.player)
        {

        }
        #endregion

        #region Passive
        #endregion

        return value;
    }

    public void ApplyDamage(UnitBehavior caster ,float dmgPercent, eDamagePerType dmgType = eDamagePerType.Atk)
    {
        double damageValue = 0;
        dmgPercent /= 100;
        float randomRate;
        double missDamage = 1;
        double criDamage = 1;
        double receiveDamage = 1;

        #region CheckMiss
        if(caster.UnitBase.GetStat(eStat.acc) < GetStat(eStat.dod))
        {
            double dodge = GetStat(eStat.dod) - caster.UnitBase.GetStat(eStat.acc);
            randomRate = Random.Range(0,100f);
            missDamage = dodge > randomRate ? 0.5f : 1;
        }
        #endregion

        #region CheckCritical
        if(missDamage != 1)
        {
            randomRate = Random.Range(0, 100f);
            criDamage = GetStat(eStat.criRate) > randomRate ? caster.UnitBase.GetStat(eStat.criDmg) : 1;
        }
        #endregion

        #region CheckDamageType
        switch (dmgType)
        {
            case eDamagePerType.Atk:
            default:
                damageValue = caster.UnitBase.GetStat(eStat.atk) * dmgPercent;
                break;
            case eDamagePerType.Def:
                damageValue = caster.UnitBase.GetStat(eStat.def) * dmgPercent;
                break;
            case eDamagePerType.AtkSpd:
                damageValue = caster.UnitBase.GetStat(eStat.atkSpd) * dmgPercent;
                break;
            case eDamagePerType.Hp:
                damageValue = caster.UnitBase.GetStat(eStat.hp) * dmgPercent;
                break;
            case eDamagePerType.CurHp:
                damageValue = curHp * dmgPercent;
                break;
            case eDamagePerType.MaxHp:
                damageValue = GetStat(eStat.hp) * dmgPercent;
                break;
        }
        #endregion

        #region Receive Damage Type
        #endregion

        #region Final Damage
        damageValue = damageValue * missDamage * criDamage * receiveDamage;
        #endregion

        curHp -= (long)damageValue;

        if (curHp <= 0)
        {
            unitState.isDead = true;
            UnitManager.Instance.RemoveUnit(unitBehavior);
        }
    }

    #region Buff
    public void AddBuff(BuffBase buff)
    {
        if(buffDic.ContainsKey(buff.getSkillEffectRecord.index) == false)
            buffDic.Add(buff.getSkillEffectRecord.index, null);

        buff.Init(buffStatDic);
        buffDic[buff.getSkillEffectRecord.index] = buff;
    }
    public void RemoveBuff(BuffBase buff)
    {
        if (buffDic.ContainsKey(buff.getSkillEffectRecord.index) == false) return;

        buff.UnInit(buffStatDic);
        buffDic[buff.getSkillEffectRecord.index] = null;
    }
    #endregion
}
