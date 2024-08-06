using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIInvenItemInfoDlg : UIDlg
{
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemName;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemDest;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemCondition;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textConfirm;
    [SerializeField, FoldoutGroup("Center")] private Image itemIcon;
    [SerializeField, FoldoutGroup("Center")] private Button buttonConfirm;

    private InvenItemInfo invenItemInfo;
    private UIInvenItemSlot uiInvenItemSlot;
    private bool isEquip;

    protected override void Awake()
    {
        base.Awake();
        buttonConfirm.onClick.AddListener(OnClickConfirm);
    }

    public virtual void Open(InvenItemInfo itemInfo, UIInvenItemSlot invenItemSlot)
    {
        base.Open();
        invenItemInfo = itemInfo;
        uiInvenItemSlot = invenItemSlot;
        ResetData();
    }

    public override void ResetData()
    {
        SetText(textItemName, invenItemInfo.GetName());
        SetText(textItemDest, invenItemInfo.GetDest());
        SetText(textItemCondition, "");
        SetIcon(itemIcon, invenItemInfo.GetIcon());

        switch (invenItemInfo.itemType)
        {
            case eItemType.Rune:
            case eItemType.Skill:
                isEquip = true;
                break;
            case eItemType.Use:
                isEquip = false;
                break;
        }
        SetText(textConfirm, TableManager.Instance.stringTable.GetText(isEquip ? 6 : 5));
    }

    private void OnClickConfirm()
    {
        if(isEquip)
            OnClickEquip();
        else
            OnClickUse();
    }
    private void OnClickEquip()
    {
        if (invenItemInfo.isEquip)
            uiInvenItemSlot.UnEquip();
        else
            uiInvenItemSlot.OnClickEquip();

        EventAction.ExcuteEvent(eEventKey.InGameInvenEquip);
        Close();
    }
    private void OnClickUse()
    {
        uiInvenItemSlot.UseItem();
        Close();
    }
}
