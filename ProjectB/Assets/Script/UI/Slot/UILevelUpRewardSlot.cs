using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UIInfinite;

public class UILevelUpRewardSlot : UIInfiniteItemSlot
{
    [SerializeField, FoldoutGroup("Content")] private Image skillIcon;
    [SerializeField, FoldoutGroup("Content")] private GameObject objSelected;
    [SerializeField, FoldoutGroup("Content")] private TextMeshProUGUI textItemName;
    [SerializeField, FoldoutGroup("Content")] private TextMeshProUGUI textItemDest;
    [SerializeField, FoldoutGroup("Content")] private Button buttonSelect;

    private UILevelUpDlg uILevelUpDlg;
    private bool isSelect;

    protected override void Awake()
    {
        base.Awake();

        buttonSelect.onClick.AddListener(OnClickSkill);
    }

    public virtual void Open(UILevelUpDlg levelUpDlg)
    {
        base.Open();
        uILevelUpDlg = levelUpDlg;
        isSelect = false;
        objSelected.SetActive(false);
    }

    public override void ResetData()
    {

    }

    public override void Close()
    {
        base.Close();
    }

    private void OnClickSkill()
    {
        CheckSlotSelect(true);
    }

    public void CheckSlotSelect(bool isOn)
    {
        if(isOn)
        {
            if (isSelect) return;
            isSelect = true;
        }
        else
        {
            isSelect = false;
        }

        objSelected.SetActive(isSelect);
    }
}
