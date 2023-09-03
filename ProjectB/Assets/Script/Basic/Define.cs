using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int UNIT_LAYER = 12;
    public const int MaxNickName = 12;

    public static string privateKey = "FHWqa8jt0hNf7e78";
}

#region Game System
public enum eTeam
{
    player,
    monster,
}

public enum eUnitType
{
    Normal,
    Uniq,
    Boss,
    Player,
}

public enum eDamageType
{
    Normal,     //�Ϲ� ������
    PerHp,      //���� ü�� �ۼ�Ʈ
    PerMaxHp,   //�ִ� ü�� �ۼ�Ʈ
}

public enum eStat
{
    hp,
    mp,
    atk,
    def,
    acc,
    atkSpd,
    moveSpd,
    atkRange,
    criRate,
    criDmg,
    END,
}

#endregion

#region Skill
public enum eSkillActivate
{
    hitRate,
    passive,
}
public enum eSkillTarget
{
    normal,
    self,
}
public enum eSkillType
{
    normalAtk,
    skillDamage,
}
public enum eSkillDuration
{
    time,
    casterAlive,
}
#endregion
