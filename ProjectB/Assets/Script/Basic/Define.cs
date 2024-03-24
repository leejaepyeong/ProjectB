using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int UNIT_LAYER = 12;   //���̾�
    public const int MaxNickName = 12;  //�г��� �ִ� ����
    public const int MaxEquipSkill = 5; //��ų ���� �ִ밹��
    public const int MaxEquipRune = 3;  //�� ���� �ִ밹��

    public const int DOT_DELAYTIME = 1; //��Ʈ ������ ������ �ð�

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
    None,
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
    Hp,

    CurHp,
    MaxHp,
}
public enum eSkillState
{
    Burn,   //ȭ�� : ���ݷ� ���
    Freeze, //����
    Fear,   //����
    Poison, //�ߵ� : ü�� ���
    Stun,   //����

    AddStat,//����
}
public enum eSkillType
{
    Normal,
    Active,
    Passive,
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
    None,
    Normal,
    Fire,
    Ice,
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
    Click_Target,
    Click_Direction,
    Target,
    Target_Direction,
    Team,
    Self,
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
    AddDmg,

    AddStat,
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
public enum eItemType
{
    None,
}

public enum eItemGrade
{
    N,
    R,
    SR,
    SSR,
}
#endregion
