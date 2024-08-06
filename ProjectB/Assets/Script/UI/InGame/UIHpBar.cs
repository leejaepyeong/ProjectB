using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIHpBar : UIBase
{
    [SerializeField, FoldoutGroup("Content")] private Slider iconHpGauge;

    [ShowInInspector] public UnitBehavior targetUnit;
    private UIHpBarDlg hpBarDlg;

    public virtual void Open(UnitBehavior unit, UIHpBarDlg dlg)
    {
        base.Open();
        targetUnit = unit;
        hpBarDlg = dlg;
        transform.parent = hpBarDlg.attach;
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        RectTransform.anchoredPosition = WorldToCanvasRectPosition();
    }

    public override void Close()
    {
        targetUnit = null;
        hpBarDlg.RecallHpBar(this);
        base.Close();
    }

    public override void UpdateFrame(float deltaTime)
    {
        iconHpGauge.value = (float)(targetUnit.UnitBase.curHp / targetUnit.UnitBase.GetStat(eStat.hp));
        RectTransform.anchoredPosition = WorldToCanvasRectPosition();

        if (targetUnit == null || targetUnit.UnitState.isDead)
            Close();
    }

    private Vector2 WorldToCanvasRectPosition()
    {
        var temp = PlayLogic.Instance.playCamera.WorldToViewportPoint(new Vector3(targetUnit.GetPos().x, targetUnit.GetPos().y + 1f, 0));
        temp.x = Mathf.LerpUnclamped(hpBarDlg.RectTransform.rect.xMin, hpBarDlg.RectTransform.rect.xMax, temp.x);
        temp.y = Mathf.LerpUnclamped(hpBarDlg.RectTransform.rect.yMin, hpBarDlg.RectTransform.rect.yMax, temp.y);

        return temp;
    }
}
