using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UILevelUpDlg : UIDlg
{
    [SerializeField, FoldoutGroup("Content")] private TextMeshProUGUI textLevel;

    public override void Open()
    {
        base.Open();

        BattleManager.Instance.isPause = true;

        ResetData();
    }

    public override void Close()
    {
        BattleManager.Instance.isPause = false;
        base.Close();
    }

    public override void ResetData()
    {
        textLevel.SetText($"Level {BattleManager.Instance.lv}");
    }

    public override void UpdateFrame(float deltaTime)
    {
    }
}
