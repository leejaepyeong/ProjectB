using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int UNIT_LAYER = 12;   //레이어
    public const int MaxNickName = 12;  //닉네임 최대 길이
    public const int MaxEquipSkill = 5; //스킬 장착 최대갯수
    public const int MaxEquipRune = 3;  //룬 장착 최대갯수

    public static string privateKey = "FHWqa8jt0hNf7e78";

    public static T Load<T>(string _path) where T : Object
    {
        if (null == _path || 0 >= _path.Length)
        {
            Debug.LogError("ResUtil::Load() [ null == _path ] : ");
            return null;
        }

        if (UIManager.Instance.ResourcePool.TryLoad<T>(_path, out var temp) == false) return null;

        return temp;
    }

    public static T GetEnum<T>(string key)
    {
        if (typeof(T).IsEnum == false)
            return default;

        return (T)System.Enum.Parse(typeof(T), key, true);
    }
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
    Normal,     //일반 데미지
    PerHp,      //현재 체력 퍼센트
    PerMaxHp,   //최대 체력 퍼센트
}

public enum eStat
{
    none,
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

public enum eLanguage
{
    Kor,
    Eng,
}

#endregion

#region Skill
public enum eDamagePerType
{
    Atk,
    Def,
    AtkSpd,
}
public enum eSkillState
{
    Burn,

    Freeze,

    AddStat,
}
public enum eSkillType
{
    Normal,
    Active,
    Passive,

    normalAtk,
    skillDamage,
}

public enum eSkillDetailType
{
    None,
    Single,
    Boom,
    Chain,
}
public enum eSkillTag
{
    Normal,
}

public enum eSkillActivate
{
    None,

    hitRate,
    passive,
}
public enum eSkillTarget
{
    Near,
    NonTarget,

    Target,
    Team,

    normal,
    self,
}
public enum eSkillDuration
{
    Time,
    Alive,
}
public enum eBuffType
{
    None,
    Buff,
    DeBuff,
}
public enum eAddType
{
    AddPer,
    AddNum,
    MinPer,
    MinNum,
}
#endregion

#region Rune
public enum eRuneType
{
    None,

    AddEffect,
    AddEffectTime,
    CoolTimeDown,
    AddProjectilenum,
    AddProjectilesize,
    AddProjectilespd,
    AddProjectiledmg,
    MinProjectilenum,
    MinProjectilesize,
    MinProjectilespd,
    MinProjectiledmg,
    AddRange,
    AddTarget,
    AddAtk,
    AddAtkspd,
    AddCrirate,
    AddCridmg,
    MinAtk,
    MinAtkspd,
    MinCrirate,
    MinCridmg,
    GetBonusExp,
    AddDmg,
    MinDmg,
}

public enum eRuneDetailType
{
    None,

    TimeAdd,

    Add,
    Atk,
}
#endregion

#region Item
public enum eItemType
{
    Skill,
    Rune,
}

public enum eItemGrade
{
    N,
    R,
    SR,
    SSR,
}
#endregion
