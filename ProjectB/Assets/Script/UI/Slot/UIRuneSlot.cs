using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class UIRuneSlot : UISlot
{
    [SerializeField, FoldoutGroup("Info")] protected Image runeIcon;
    [SerializeField, FoldoutGroup("Info")] protected Image OutLineIcon;
    [SerializeField, FoldoutGroup("Info")] protected int slotIdx;

    private RuneRecord runeRecord;
    private UnityAction<int> equipAction;

    protected override void Awake()
    {
        base.Awake();
        onClickAction = OnClickRune;
    }

    public virtual void Open(RuneRecord runeRecord, UnityAction<int> action)
    {
        base.Open();
        this.runeRecord = runeRecord;
        equipAction = action;
        ResetData();
    }

    public override void ResetData()
    {
        SetIcon(runeIcon, runeRecord != null ? runeRecord.iconPath : "");
    }

    private void OnClickRune()
    {
        if (equipAction != null)
            equipAction.Invoke(slotIdx);
    }
}
