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

    [SerializeField, FoldoutGroup("Content")] private UILevelUpDlg uILevelUpDlg;
    private bool isSelect;
    public LevelUpRewardRecord levelUpReward;

    protected override void Awake()
    {
        base.Awake();

        buttonSelect.onClick.AddListener(OnClickSkill);
    }

    public override void UpdateItemSlot(int index)
    {
        levelUpReward = uILevelUpDlg.levelUpRewardList[index];
        if (levelUpReward == null) return;

        base.UpdateItemSlot(index);

        ResetData();
    }

    public override void ResetData()
    {
        SetInfo();
        isSelect = false;
        objSelected.SetActive(false);
    }

    #region Reward Info
    private void SetInfo()
    {
        switch (levelUpReward.itemType)
        {
            case eLevelUpReward.None:
                break;
            case eLevelUpReward.Rune:
                SetRuneInfo();
                break;
            case eLevelUpReward.Passive:
            case eLevelUpReward.Active:
                SetSkillInfo();
                break;
            case eLevelUpReward.Use:
            case eLevelUpReward.Stat:
                SetStatInfo();
                break;
            default:
                break;
        }
    }

    private void SetRuneInfo()
    {
        var record = TableManager.Instance.runeTable.GetRecord(levelUpReward.itemIndex);
        if (record == null) return;

        SetIcon(skillIcon, record.iconPath);
        SetText(textItemName, record.getName);
        SetText(textItemDest, record.getDest);
    }
    private void SetSkillInfo()
    {
        var record = TableManager.Instance.skillTable.GetRecord(levelUpReward.itemIndex);
        if (record == null) return;

        SetIcon(skillIcon, record.iconPath);
        SetText(textItemName, record.getName);
        SetText(textItemDest, record.getDest);
    }
    private void SetStatInfo()
    {
        var record = TableManager.Instance.statRewardTable.GetRecord(levelUpReward.itemIndex);
        if (record == null) return;

        SetIcon(skillIcon, record.iconPath);
        SetText(textItemName, record.getName);
        SetText(textItemDest, record.getDest);
    }

    #endregion


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
            uILevelUpDlg.SelectReward(this);
        }
        else
        {
            isSelect = false;
        }

        objSelected.SetActive(isSelect);
    }
}
