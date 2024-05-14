using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UIInfinite;

public class UIInvenItemSlot : UIInfiniteItemSlot
{
    [SerializeField, FoldoutGroup("Center")] private Image itemIcon;
    [SerializeField, FoldoutGroup("Center")] private Image equipIcon;

    [SerializeField] private UIInGameInventory uiInGameInventory;

    private InvenItemInfo invenItemInfo;
    private UISkillSlot equipSkillSlot;
    private bool isEquip;

    protected override void Awake()
    {
        onClickAction = OnClickItem;
        base.Awake();
    }

    public override void ResetData()
    {
        SetIcon(itemIcon, invenItemInfo.GetIcon());
        equipIcon.gameObject.SetActive(isEquip);
    }

    public override void UpdateItemSlot(int index)
    {
        invenItemInfo = uiInGameInventory.InvenItemList[index];
        ResetData();
    }

    private void OnClickItem()
    {
        var dlg = uiManager.OpenWidget<UIInvenItemInfoDlg>();
        dlg.Open(invenItemInfo, this);
    }

    public void OnClickEquip()
    {
        PlayLogic.Instance.uiPlayLogic.uiSkillInven_Placement.Open(this);
    }

    public void Equip(UISkillSlot skillSlot)
    {
        equipSkillSlot = skillSlot;
        isEquip = true;
        ResetData();
    }

    public void UnEquip()
    {
        equipSkillSlot.OnClickUnEquip();
        isEquip = false;
        ResetData();
    }
    public void UseItem()
    {
        BattleManager.Instance.playerData.RemoveItem(invenItemInfo.itemId);
        ResetData();
    }
    public void SellItem()
    {
        BattleManager.Instance.playerData.RemoveItem(invenItemInfo.itemId);
        ResetData();
    }

    public SkillRecord getSkillRecord => invenItemInfo.GetSkillRecord();
    public RuneRecord getRuneRecord => invenItemInfo.GetRuneRecord();
    public eItemType itemType => invenItemInfo.itemType;
}
