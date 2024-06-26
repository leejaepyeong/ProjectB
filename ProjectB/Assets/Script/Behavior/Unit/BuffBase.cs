using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
{
    protected SkillEffectRecord skillEffectRecord;
    protected UnitBehavior caster;
    protected UnitBehavior target;
    protected float elaspedTime;

    public BuffBase(SkillEffectRecord skillEffect, UnitBehavior caster, UnitBehavior target)
    {
        skillEffectRecord = skillEffect;
        this.caster = caster;
        this.target = target;
    }

    public virtual void Init()
    {
        elaspedTime = 0;
    }

    public virtual void UnInit()
    {
        elaspedTime = 0;
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        elaspedTime += deltaTime;
    }

    public bool CheckEndBuff()
    {
        switch (skillEffectRecord.skillDurationType)
        {
            case eSkillDuration.Time:
                return elaspedTime >= skillEffectRecord.skillDuration;
            case eSkillDuration.Alive:
                return caster != null && caster.UnitState.isDead == false;
            default:
                return true;
        }
    }

    public SkillEffectRecord getSkillEffectRecord => skillEffectRecord;
}

#region AddStat
public class BuffBase_AddStat : BuffBase
{
    public BuffBase_AddStat(SkillEffectRecord skillEffect, UnitBehavior caster, UnitBehavior target) : base(skillEffect, caster, target)
    {
    }
}
#endregion

#region Frozen
public class BuffBase_Frozen: BuffBase
{
    public BuffBase_Frozen(SkillEffectRecord skillEffect, UnitBehavior caster, UnitBehavior target) : base(skillEffect, caster, target)
    {
    }
}
#endregion
#region Stun
public class BuffBase_Stun : BuffBase
{
    public BuffBase_Stun(SkillEffectRecord skillEffect, UnitBehavior caster, UnitBehavior target) : base(skillEffect, caster, target)
    {
    }
}
#endregion

#region Fear
public class BuffBase_Fear : BuffBase
{
    public BuffBase_Fear(SkillEffectRecord skillEffect, UnitBehavior caster, UnitBehavior target) : base(skillEffect, caster, target)
    {
    }
    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
    }
}
#endregion

#region Burn
public class BuffBase_Burn : BuffBase
{
    private float coolTime;
    private SkillEffectRecord.SkillEffectStat dotEffectStat;
    public BuffBase_Burn(SkillEffectRecord skillEffect, UnitBehavior caster, UnitBehavior target) : base(skillEffect, caster, target)
    {
    }
    public override void Init()
    {
        base.Init();
        dotEffectStat = skillEffectRecord.skillEffectStatList.Find(_ => _.stat == eStat.DotDmg);
        coolTime = 0;
    }
    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        if (dotEffectStat == null) return;

        coolTime += deltaTime;
        if (coolTime > Define.DOT_DELAYTIME)
        {
            ApplyDamage();
            coolTime = 0;
        }
    }
    private void ApplyDamage()
    {
        target.UnitBase.ApplyDamage(caster, dotEffectStat.value, eDamagePerType.Atk);
    }
}
#endregion

#region Poison
public class BuffBase_Poison : BuffBase
{
    private float coolTime;
    private SkillEffectRecord.SkillEffectStat dotEffectStat;
    public BuffBase_Poison(SkillEffectRecord skillEffect, UnitBehavior caster, UnitBehavior target) : base(skillEffect, caster, target)
    {
    }
    public override void Init()
    {
        base.Init();
        dotEffectStat = skillEffectRecord.skillEffectStatList.Find(_ => _.stat == eStat.DotDmg);
        coolTime = 0;
    }
    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        if (dotEffectStat == null) return;
        
        coolTime += deltaTime;
        if (coolTime > Define.DOT_DELAYTIME)
        {
            ApplyDamage();
            coolTime = 0;
        }
    }
    private void ApplyDamage()
    {
        target.UnitBase.ApplyDamage(caster, dotEffectStat.value, eDamagePerType.MaxHp);
    }
}
#endregion