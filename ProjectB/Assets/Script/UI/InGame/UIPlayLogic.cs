using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIPlayLogic : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Top")] private UIWaveInfo uiWaveInfo;
    [SerializeField, FoldoutGroup("Top_Right")] private Button btnOption;
    [SerializeField, FoldoutGroup("Top_Right")] private Button btnInven;
    [SerializeField, FoldoutGroup("Top_Left")] private Button btnStat;

    [SerializeField, FoldoutGroup("Bottom")] private UISkillGroup uiSkillGroup;

    public void Init()
    {
        btnOption.onClick.AddListener(OnClickGamePause);
        btnInven.onClick.AddListener(OnClickInventory);
        btnStat.onClick.AddListener(OnClickStatInfo);
        uiSkillGroup.Init();
    }

    public void UpdateFrame(float deltaTime)
    {
        uiWaveInfo.UpdateFrame(deltaTime);
        CheckKeyCode();
    }

    private void CheckKeyCode()
    {
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
            OnClickGamePause();
    }

    #region Button Click
    private void OnClickGamePause()
    {
        if (BattleManager.Instance.isPause) return;
        UIManager.Instance.OpenWidget<UIPauseDlg>();
        BattleManager.Instance.isPause = true;
    }

    private void OnClickStatInfo()
    {

    }
    private void OnClickInventory()
    {
        UIManager.Instance.OpenWidget<UIInGameInventory>();
    }
    #endregion
}
