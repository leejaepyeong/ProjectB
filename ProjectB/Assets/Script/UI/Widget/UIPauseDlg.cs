using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;

public class UIPauseDlg : UIBase
{
    [FoldoutGroup("Center")]
    [SerializeField, FoldoutGroup("Center/GameSetting")] private Toggle[] toggleTabs;

    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private Scrollbar scrollAll;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private Scrollbar scrollBgm;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private Scrollbar scrollEffect;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private TextMeshProUGUI textAllValue;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private TextMeshProUGUI textBgmValue;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private TextMeshProUGUI textEffectValue;

    [SerializeField, FoldoutGroup("Bottom")] private Button buttonRestart;
    [SerializeField, FoldoutGroup("Bottom")] private Button buttonBackGame;
    [SerializeField, FoldoutGroup("Bottom")] private Button buttonOutGame;

    protected override void Awake()
    {
        base.Awake();
        buttonRestart.onClick.AddListener(OnClickRestart);
        buttonBackGame.onClick.AddListener(OnClickBacktoGame);
        buttonOutGame.onClick.AddListener(OnClickOutGame);

        for (int i = 0; i < toggleTabs.Length; i++)
        {
            toggleTabs[i].onValueChanged.AddListener(OnValueChageTab);
        }
    }

    public override void Open()
    {
        base.Open();
        ResetData();
    }

    public override void ResetData()
    {

    }

    public override void Close()
    {
        BattleManager.Instance.isPause = false;
        base.Close();
    }

    #region Button Click
    private void OnClickRestart()
    {

    }
    private void OnClickBacktoGame()
    {

    }
    private void OnClickOutGame()
    {

    }
    private void OnValueChageTab(bool isOn)
    {
        if (isOn)
        {

        }
        else
        {

        }
    }
    #endregion

    #region Sound

    #endregion
}
