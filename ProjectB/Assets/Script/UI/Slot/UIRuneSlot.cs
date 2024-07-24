using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class UIRuneSlot : UISlot
{
    [SerializeField, FoldoutGroup("Info")] protected GameObject objSelect;
    [SerializeField, FoldoutGroup("Info")] protected Button buttonClickRune;
    [SerializeField, FoldoutGroup("Info")] protected Image runeIcon;
    [SerializeField, FoldoutGroup("Info")] protected Image OutLineIcon;
    [SerializeField, FoldoutGroup("Info")] protected int slotIdx;

    private RuneRecord runeRecord;
    private UnityAction<int> equipAction;

    protected override void Awake()
    {
        base.Awake();
        buttonClickRune.onClick.AddListener(OnClickRune);
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
        runeIcon.gameObject.SetActive(runeRecord != null);
        if (runeRecord != null)
            SetIcon(runeIcon, runeRecord.iconPath);
    }

    private void OnClickRune()
    {
        if (equipAction != null)
            equipAction.Invoke(slotIdx);
    }
}
