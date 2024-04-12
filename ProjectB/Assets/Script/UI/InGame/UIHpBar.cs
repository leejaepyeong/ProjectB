using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIHpBar : UIBase
{
    [SerializeField, FoldoutGroup("Content")] private Image iconHpGauge;

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
        var temp = Manager.Instance.MainCamera.WorldToViewportPoint(new Vector3(targetUnit.GetPos().x, targetUnit.GetPos().y + 1f, 0));
        temp.x = Mathf.LerpUnclamped(hpBarDlg.RectTransform.rect.xMin, hpBarDlg.RectTransform.rect.xMax, temp.x);
        temp.y = Mathf.LerpUnclamped(hpBarDlg.RectTransform.rect.yMin, hpBarDlg.RectTransform.rect.yMax, temp.y);
        RectTransform.anchoredPosition = temp;

        if (targetUnit == null || targetUnit.UnitState.isDead)
            Close();
    }

    private Vector2 WorldToCanvasRectPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        Vector2 temp = camera.WorldToViewportPoint(position);

        var canvasRect = canvas.rect;
        temp.x = Mathf.LerpUnclamped(canvasRect.xMin, canvasRect.xMax, temp.x);
        temp.y = Mathf.LerpUnclamped(canvasRect.yMin, canvasRect.yMax, temp.y);

        return temp;
    }
}
