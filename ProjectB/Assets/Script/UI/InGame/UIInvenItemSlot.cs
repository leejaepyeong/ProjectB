using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIInvenItemSlot : UISlot
{
    [SerializeField, FoldoutGroup("")] private Image itemIcon;
    [SerializeField, FoldoutGroup("")] private Image equipIcon;
    [SerializeField, FoldoutGroup("")] private Button btnEquip;
    [SerializeField, FoldoutGroup("")] private Button btnUnEquip;

    private UIInGameInventory uiInGameInventory;
    private int slotIdx;

    protected override void Awake()
    {
        btnEquip.onClick.AddListener(Equip);
        btnUnEquip.onClick.AddListener(UnEquip);
    }

    public virtual void Open(InvenItemInfo itemInfo)
    {
        base.Open();
        if (uiInGameInventory == null) return;

    }

    public void Set(UIInGameInventory uiInGameInventory)
    {
        this.uiInGameInventory = uiInGameInventory;
    }

    public void Equip()
    {

    }

    public void UnEquip()
    {

    }
}
