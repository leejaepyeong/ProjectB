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
    [SerializeField, FoldoutGroup("Top_Right")] private Button btnStat;
    [SerializeField, FoldoutGroup("Top_Right")] private Button btnMission;


    [SerializeField, FoldoutGroup("Top_Left")] private TextMeshProUGUI userNickName;
    [SerializeField, FoldoutGroup("Top_Left")] private TextMeshProUGUI userLevel;

    [FoldoutGroup("Bottom")] public UISkillInven uiSkillInven;
    [FoldoutGroup("Bottom")] public UISkillInven_Placement uiSkillInven_Placement;
    [SerializeField, FoldoutGroup("Bottom/Exp")] private Slider expGage;
    [SerializeField, FoldoutGroup("Bottom/Exp")] private TextMeshProUGUI textExp;

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

        expGage.value = 0;
        textExp.SetText("0");
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

    public void UpdateExp()
    {
        float percent = (float)BattleManager.Instance.playerData.curExp / BattleManager.Instance.playerData.needExp;
        expGage.value = percent;
        textExp.SetText((percent * 100).ToString("F1"));
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
        var dlg = UIManager.Instance.OpenWidget<UIInGameInventory>();
        dlg.Open();
    }
    private void OnClickInventory()
    {
        var dlg = UIManager.Instance.OpenWidget<UIInGameInventory>();
        dlg.Open();
    }
    #endregion
}
