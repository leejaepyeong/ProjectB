using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UILevelUpDlg : UIDlg
{
    [SerializeField, FoldoutGroup("Content")] private TextMeshProUGUI textLevel;
    [SerializeField, FoldoutGroup("Content")] private Button buttonConfirm;
    [SerializeField, FoldoutGroup("Content")] private UIInfinite.UIInfiniteScroll infiniteScroll;

    public List<LevelUpRewardRecord> levelUpRewardList = new List<LevelUpRewardRecord>();
    private UILevelUpRewardSlot selectRewardSlot;

    protected override void Awake()
    {
        base.Awake();
        buttonConfirm.onClick.AddListener(OnClickSelectComplete);
    }

    public virtual void Open(int levelUpCount)
    {
        base.Open();

        BattleManager.Instance.isPause = true;
        selectRewardSlot = null;

        ResetData();
    }

    public override void Close()
    {
        BattleManager.Instance.isPause = false;
        base.Close();
    }

    public override void ResetData()
    {
        textLevel.SetText($"Level {BattleManager.Instance.playerData.curLv}");

        SetRewardList();
    }

    private void SetRewardList()
    {
        var expRecord =  TableManager.Instance.expTable.GetExpRecord(BattleManager.Instance.playerData.curLv);
        levelUpRewardList = TableManager.Instance.levelUpRewardTable.GetLevelUpRewardList(expRecord.rewardIdx, 3);

        infiniteScroll.Set(levelUpRewardList.Count);
    }

    public void SelectReward(UILevelUpRewardSlot rewardSlot)
    {
        if (selectRewardSlot != null)
            selectRewardSlot.CheckSlotSelect(false);
        selectRewardSlot = rewardSlot;
    }

    private void OnClickSelectComplete()
    {
        if (selectRewardSlot == null)
        {
            uiManager.OpenMessageBox_Ok("알림", "보상을 선택하세요.");
            return;
        }

        switch (selectRewardSlot.levelUpReward.itemType)
        {
            case eLevelUpReward.Rune:
                BattleManager.Instance.playerData.AddRuneItem(selectRewardSlot.levelUpReward.itemIndex);
                break;
            case eLevelUpReward.Passive:
            case eLevelUpReward.Active:
                BattleManager.Instance.playerData.AddISkilltem(selectRewardSlot.levelUpReward.itemIndex);
                break;
            case eLevelUpReward.Use:
            case eLevelUpReward.Stat:
                BattleManager.Instance.playerData.AddUseItem(selectRewardSlot.levelUpReward.itemIndex);
                break;
            default:
                Debug.LogError("No Type LevelUpRewardItem");
                break;
        }

        Close();
    }
}
