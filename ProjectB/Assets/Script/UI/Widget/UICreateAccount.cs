using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UICreateAccount : UIBase
{
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textTitle;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textDest;
    [SerializeField, FoldoutGroup("Center")] private TMP_InputField textInputNick;
    [SerializeField, FoldoutGroup("Center")] private Button buttonOk;
    [SerializeField, FoldoutGroup("Center")] private Button buttonCancle;

    private string title;
    private string dest;
    private int maxLength = 12;

    public class UIParameter : Parameter
    {
        public string title;
        public string dest;
        public string ok;
        public string cancle;
        public bool escapable = true;
        public bool backable = true;
    }

    public override void SetParam(Parameter param)
    {
        if (param is UIParameter parameter)
        {
            title = parameter.title;
            dest = parameter.dest;
            escapable = parameter.escapable;
            backable = parameter.backable;
        }
    }

    public override void Init()
    {
        base.Init();
        buttonOk.onClick.AddListener(OnClickConfirm);
        buttonCancle.onClick.AddListener(OnClickCancle);
    }

    public override void DeInit()
    {
        buttonOk.onClick.RemoveAllListeners();
        buttonCancle.onClick.RemoveAllListeners();
    }
    public override void Open()
    {
        base.Open();

        SetText(textTitle, title);
        SetText(textDest, dest);
    }

    #region Button Click
    private void OnClickConfirm()
    {
        UIMessageBox.UIParameter parameter = new UIMessageBox.UIParameter();

        if (CheckNickName(textInputNick.text) == false)
        {
            parameter.title = "";
            parameter.dest = "";

            uiManager.OpenUI<UIMessageBox>();
        }
        else
        {
            parameter.title = "";
            parameter.dest = "";
            uiManager.OpenUI<UIMessageBox>(parameter);
        }
    }

    private void OnClickCancle()
    {

    }
    #endregion

    private bool CheckNickName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (name.Length > maxLength) return false;

        return true;
    }
}
