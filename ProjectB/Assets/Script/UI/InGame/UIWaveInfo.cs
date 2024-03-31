using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIWaveInfo : UIDlg
{
    [SerializeField, FoldoutGroup("Time")] private TextMeshProUGUI textTimer;

    public override void UpdateFrame(float deltaTime)
    {
        SetTime();
    }

    public void SetTime()
    {
        SetText(textTimer, BattleManager.Instance.getCurTime.ToString("F2"));
    }
}
