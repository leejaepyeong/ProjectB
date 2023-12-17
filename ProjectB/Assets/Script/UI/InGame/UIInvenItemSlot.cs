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
    private int slotIdx;
    private int runeIdx;
    private bool isEquip;

    protected override void Awake()
    {
        base.Awake();
        onClickAction = OnClickItem;
    }

    public bool isRune => invenItemInfo.isRune;

    public virtual void Open(InvenItemInfo itemInfo, UIInGameInventory uiInGameInventory)
    {
        base.Open();
        this.uiInGameInventory = uiInGameInventory;
        invenItemInfo = itemInfo;
        ResetData();
    }

    public override void ResetData()
    {
        SetIcon(itemIcon, invenItemInfo.GetIcon());
        equipIcon.gameObject.SetActive(isEquip);
    }

    private void OnClickItem()
    {
        var dlg = uiManager.OpenWidget<UIInvenItemInfoDlg>();
        dlg.Open(invenItemInfo, this);
    }

    public void TryEquip()
    {
        PlayLogic.Instance.uiPlayLogic.uiSkillInven_Placement.Open(this);
    }

    public void Equip()
    {
        isEquip = true;
        ResetData();
    }

    public void UnEquip()
    {
        isEquip = false;
        ResetData();
    }

    public SkillRecord getSkillRecord => invenItemInfo.GetSkillRecord();
    public RuneRecord getRuneRecord => invenItemInfo.GetRuneRecord();
}
