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
    user,
    enemy,
}

public enum eUnitType
{
    Normal,
    Uniq,
    Boss,
}

public enum eDamageType
{
    Normal,     //일반 데미지
    PerHp,      //현재 체력 퍼센트
    PerMaxHp,   //최대 체력 퍼센트
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
