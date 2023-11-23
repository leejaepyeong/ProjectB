using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIItemInfoDlg : UIBase
{
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemName;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemDest;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textItemCondition;
    [SerializeField, FoldoutGroup("Center")] private Image itemIcon;
    [SerializeField, FoldoutGroup("Bottom")] private Button buttonEquip;

    private InvenItemInfo invenItemInfo;
    private UIInvenItemSlot uiInvenItemSlot;

    protected override void Awake()
    {
        base.Awake();
        buttonEquip.onClick.AddListener(OnClickEquip);
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
        SetText(textItemName, invenItemInfo.getItemRecord.getName);
        SetText(textItemDest, invenItemInfo.getItemRecord.getDest);
        SetText(textItemCondition, "");
        SetIcon(itemIcon, invenItemInfo.getItemRecord.iconPath);
    }

    private void OnClickEquip()
    {
        uiInvenItemSlot.Equip();
    }
}
