using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIInvenItemInfoDlg : UIBase
{
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemName;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemDest;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemCondition;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textEquip;
    [SerializeField, FoldoutGroup("Center")] private Image itemIcon;

    private InvenItemInfo invenItemInfo;
    private UIInvenItemSlot uiInvenItemSlot;

    protected override void Awake()
    {
        base.Awake();
        onClickAction = OnClickEquip;
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

        SetText(textEquip, TableManager.Instance.stringTable.GetText(invenItemInfo.isEquip ? 1 : 2));
    }

    private void OnClickEquip()
    {
        if (invenItemInfo.isEquip)
            uiInvenItemSlot.Equip();
        else
            uiInvenItemSlot.UnEquip();
    }
}
