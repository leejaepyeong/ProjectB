using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIHpBar : UIBase
{
    [SerializeField, FoldoutGroup("Content")] private Image iconHpGauge;

    private UnitBehavior targetUnit;
    private UIHpBarDlg hpBarDlg;

    public virtual void Open(UnitBehavior unit, UIHpBarDlg dlg)
    {
        base.Open();
        targetUnit = unit;
        hpBarDlg = dlg;
    }

    public override void Close()
    {
        targetUnit = null;
        hpBarDlg.RecallHpBar(this);
        base.Close();
    }

    public override void UpdateFrame(float deltaTime)
    {
        iconHpGauge.fillAmount = (float)(targetUnit.UnitBase.curHp / targetUnit.UnitBase.GetStat(eStat.hp));

    }
}
