using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int UNIT_LAYER = 12;   //레이어
    public const int MaxNickName = 12;  //닉네임 최대 길이
    public const int MaxEquipSkill = 5; //스킬 장착 최대갯수
    public const int MaxEquipRune = 3;  //룬 장착 최대갯수

    public const int DOT_DELAYTIME = 1; //도트 데미지 딜레이 시간

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

    public static int GetTargetLayer(eTeam team)
    {
        int layer = team == eTeam.player ? LayerMask.GetMask("Monster") : LayerMask.GetMask("Player");
        return layer;
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

public enum eStat
{
    hp,
    mp,
    atk,
    def,
    acc,
    dod,
    atkSpd,
    moveSpd,
    atkRange,
    criRate,
    criDmg,
    END,
    DotDmg,
    EXP,
    None,
}
public enum eUseType
{
    AddExp,
    AddSlot,
    None,
}

public enum eLanguage
{
    Kor,
    Eng,
}

public enum eEventKey
{
    //Inventory
    InGameInvenEquip,

}

public enum ePlayLogicFsm
{
    none,
    setting,
    ready,
    play,
    bossRound,
    result,
}

public enum eUnitFsm
{
    idle,           //기본상태
    attack,         //공격
    skill_NonStop,  //스킬 캔슬 불가
    skill,          //스킬
    abnormal,       //상태이상
    die,            //사망
}

#endregion

#region Skill
public enum eDamagePerType
{
    Atk,
    Def,
    AtkSpd,
    Hp,

    CurHp,
    MaxHp,
}
public enum eSkillState
{
    Burn,   //화상 : 공격력 비례
    Freeze, //빙결
    Fear,   //공포
    Poison, //중독 : 체력 비례
    Stun,   //기절

    AddStat,//스탯
}
public enum eSkillType
{
    Normal,
    Active,
    Auto,
    Passive,
}

public enum eSkillDetailType
{
    Normal,
    NonCancle,
}
public enum eSkillTag
{
    None,
    Normal,
    Fire,
    Ice,
}

public enum eSkillCondition
{
    None,

    attackRate,
    hitState,
    stack,
}
public enum eSkillTarget
{
    Near,
    NonTarget,
    Click_Target,
    Click_Direction,
    Team,
    Random,
    Random_Overlap,
    Self,
}
public enum eSkillDuration
{
    Time,
    Alive,
    Eternity,
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
    AddDmg,

    AddAtk,
    AddDamage,
    GetBonusExp,
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
public enum eLevelUpReward
{
    None,
    Rune,
    Passive,
    Active,
    Use,
    Stat,
}
public enum eItemType
{
    None,
    Rune,
    Skill,
    Use,
}

public enum eItemGrade
{
    N,
    R,
    SR,
    SSR,
}
#endregion

#region Stage
public enum eStageType
{
    Normal,
    Wave,
}
#endregion
