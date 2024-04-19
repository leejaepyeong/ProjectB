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

        Close();
    }
}
