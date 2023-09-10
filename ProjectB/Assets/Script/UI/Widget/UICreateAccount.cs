using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UICreateAccount : UIBase
{
    public enum eNickDeny
    {
        none,
        empty,
        maxLength,
    }

    [SerializeField, FoldoutGroup("Center")] private TMP_InputField textInputNick;
    [SerializeField, FoldoutGroup("Center")] private Button buttonOk;
    [SerializeField, FoldoutGroup("Center")] private Button buttonCancle;

    private eNickDeny nickState;

    public class UIParameter : Parameter
    {
        public string ok;
        public string cancle;
        public bool escapable = true;
        public bool backable = true;
    }

    public override void SetParam(Parameter param)
    {
        if (param is UIParameter parameter)
        {
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
    }

    #region Button Click
    private void OnClickConfirm()
    {
        UIMessageBox.UIParameter parameter = new UIMessageBox.UIParameter();
        CheckNickName(textInputNick.text);

        switch (nickState)
        {
            case eNickDeny.none:
                parameter.title = Manager.Instance.GetString(1000);
                parameter.dest = string.Format(Manager.Instance.GetString(1001), textInputNick);
                UserInfo userInfo = new UserInfo(textInputNick.text);
                parameter.okAction = () => SaveData_Local.Instance.SetUserInfo(userInfo); SaveData_PlayerSkill.Instance.SetSkillInfo(); Close();
                break;
            case eNickDeny.empty:
                parameter.title = Manager.Instance.GetString(1000);
                parameter.dest = Manager.Instance.GetString(1002);
                break;
            case eNickDeny.maxLength:
                parameter.title = Manager.Instance.GetString(1000);
                parameter.dest = Manager.Instance.GetString(1003);
                break;
        }
        uiManager.OpenUI<UIMessageBox>(parameter);
    }

    private void OnClickCancle()
    {

    }
    #endregion

    private void CheckNickName(string name)
    {
        if (string.IsNullOrEmpty(name)) { nickState = eNickDeny.empty; return; };
        if (name.Length > Define.MaxNickName) {nickState = eNickDeny.maxLength ; return; };

        nickState = eNickDeny.none;
        return;
    }
}
