using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
{
    private SkillEffectRecord skillEffectRecord;
    private UnitBehavior caster;
    private float elaspedTime;

    public BuffBase(SkillEffectRecord skillEffect, UnitBehavior caster)
    {
        skillEffectRecord = skillEffect;
        this.caster = caster;
    }

    public void Init()
    {
        elaspedTime = 0;
    }

    public void UpdateFrame(float deltaTime)
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
}
