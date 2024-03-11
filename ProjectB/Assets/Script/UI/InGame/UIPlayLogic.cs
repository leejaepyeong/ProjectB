using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIPlayLogic : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [SerializeField, FoldoutGroup("Top")] private UIWaveInfo uiWaveInfo;
    [SerializeField, FoldoutGroup("Top_Right")] private Button btnOption;
    [SerializeField, FoldoutGroup("Top_Right")] private Button btnInven;
    [SerializeField, FoldoutGroup("Top_Left")] private Button btnStat;

    [FoldoutGroup("Bottom")] public UISkillInven uiSkillInven;
    [FoldoutGroup("Bottom")] public UISkillInven_Placement uiSkillInven_Placement;
    [FoldoutGroup("Bottom/Exp")] public Image expGage;
    [FoldoutGroup("Bottom/Exp")] public TextMeshProUGUI textExp;

    private void Awake()
    {
        canvas.worldCamera = UIManager.Instance.UiCamera;
    }

    public void Init()
    {
        btnOption.onClick.AddListener(OnClickGamePause);
        btnInven.onClick.AddListener(OnClickInventory);
        btnStat.onClick.AddListener(OnClickStatInfo);
        uiSkillInven.Init();
        uiSkillInven_Placement.Init();
    }

    public void UpdateFrame(float deltaTime)
    {
        uiWaveInfo.UpdateFrame(deltaTime);
        uiSkillInven.UpdateFrame(deltaTime);
        uiSkillInven_Placement.UpdateFrame(deltaTime);
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
        var dlg = UIManager.Instance.OpenWidget<UIPauseDlg>();
        dlg.Open();
        BattleManager.Instance.isPause = true;
    }

    private void OnClickStatInfo()
    {

    }
    private void OnClickInventory()
    {
        UIManager.Instance.OpenWidget<UIInGameInventory>();
        var dlg = UIManager.Instance.OpenWidget<UIPauseDlg>();
        dlg.Open();
    }
    #endregion
}
