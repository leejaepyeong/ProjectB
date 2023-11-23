using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIInvenItemSlot : UISlot
{
    [SerializeField, FoldoutGroup("Center")] private Image itemIcon;
    [SerializeField, FoldoutGroup("Center")] private Image equipIcon;

    private UIInGameInventory uiInGameInventory;
    private InvenItemInfo invenItemInfo;
    private ItemRecord itemRecord;
    private int slotIdx;
    private bool isEquip;

    protected override void Awake()
    {
        base.Awake();
        onClickAction = OnClickItem;
    }

    public virtual void Open(InvenItemInfo itemInfo, UIInGameInventory uiInGameInventory)
    {
        base.Open();
        this.uiInGameInventory = uiInGameInventory;
        invenItemInfo = itemInfo;
        itemRecord = invenItemInfo.getItemRecord;
        ResetData();
    }

    public override void ResetData()
    {
        SetIcon(itemIcon, itemRecord.iconPath);
    }

    private void OnClickItem()
    {
        var dlg = uiManager.OpenWidget<UIItemInfoDlg>();
        dlg.Open(invenItemInfo, this);
    }

    public void Equip()
    {
        isEquip = true;
        if(invenItemInfo.isRune)
        {
            uiInGameInventory.EquipRune();
        }
        else
        {
            uiInGameInventory.EquipSkill();
        }
    }

    public void UnEquip()
    {
        isEquip = false;
    }
}
