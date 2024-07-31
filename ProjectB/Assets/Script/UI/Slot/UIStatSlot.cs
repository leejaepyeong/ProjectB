using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIStatSlot : UISlot
{
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textBasicStat;
    [SerializeField] private TextMeshProUGUI textAddStat;

    private UnitBehavior unit;
    private eStat statType;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual void Open(UnitBehavior unit, eStat statType)
    {
        base.Open();
        this.unit = unit;
        this.statType = statType;
        BattleManager.Instance.isPause = true;
    }

    public override void Close()
    {
        BattleManager.Instance.isPause = false;
        base.Close();
    }

    public override void ResetData()
    {
        base.ResetData();
        SetText(textName, GetStatString());
        SetText(textBasicStat, $"{unit.UnitBase.GetStat(statType)}");
        SetText(textAddStat, $"{unit.UnitBase.GetStat(statType) - unit.UnitState.originStatValue[statType]}");
    }

    private string GetStatString()
    {
        string name = "";
        switch (statType)
        {
            case eStat.hp: TableManager.Instance.stringTable.GetText(4005); break;
            case eStat.mp: TableManager.Instance.stringTable.GetText(4007); break;
            case eStat.atk: TableManager.Instance.stringTable.GetText(4002); break;
            case eStat.def: TableManager.Instance.stringTable.GetText(4003); break;
            case eStat.acc: TableManager.Instance.stringTable.GetText(4010); break;
            case eStat.dod: TableManager.Instance.stringTable.GetText(4011); break;
            case eStat.atkSpd: TableManager.Instance.stringTable.GetText(4012); break;
            case eStat.moveSpd: TableManager.Instance.stringTable.GetText(4013); break;
            case eStat.atkRange: TableManager.Instance.stringTable.GetText(4014); break;
            case eStat.criRate: TableManager.Instance.stringTable.GetText(4008); break;
            case eStat.criDmg: TableManager.Instance.stringTable.GetText(4009); break;
        }
        return name;
    }
}
