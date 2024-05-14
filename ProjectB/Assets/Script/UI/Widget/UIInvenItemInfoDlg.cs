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
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textEquip;
    [SerializeField, FoldoutGroup("Center")] private Image itemIcon;
    [SerializeField, FoldoutGroup("Center")] private Button buttonEquip;
    [SerializeField, FoldoutGroup("Center")] private Button buttonUse;



    private InvenItemInfo invenItemInfo;
    private UIInvenItemSlot uiInvenItemSlot;

    protected override void Awake()
    {
        base.Awake();
        buttonEquip.onClick.AddListener(OnClickEquip);
        buttonUse.onClick.AddListener(OnClickUse);
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
                buttonEquip.gameObject.SetActive(true);
                buttonUse.gameObject.SetActive(false);
                break;
            case eItemType.Use:
                buttonEquip.gameObject.SetActive(true);
                buttonUse.gameObject.SetActive(true);
                break;
        }
        SetText(textEquip, TableManager.Instance.stringTable.GetText(invenItemInfo.isEquip ? 6 : 5));
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
