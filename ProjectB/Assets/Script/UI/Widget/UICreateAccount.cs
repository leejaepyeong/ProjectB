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
        noUseText,
    }

    [SerializeField, FoldoutGroup("Center")] private TMP_InputField textInputNick;
    [SerializeField, FoldoutGroup("Center")] private Button buttonOk;
    [SerializeField, FoldoutGroup("Center")] private Button buttonCancle;

    private eNickDeny nickState;

    protected override void Awake()
    {
        base.Awake();
        buttonOk.onClick.AddListener(OnClickConfirm);
        buttonCancle.onClick.AddListener(OnClickCancle);
    }

    #region Button Click
    private void OnClickConfirm()
    {
        CheckNickName(textInputNick.text);
        string title = "";
        string dest = "";
        UnityEngine.Events.UnityAction okAction = null;

        switch (nickState)
        {
            case eNickDeny.none:
                title = TableManager.Instance.stringTable.GetText(1000);
                dest = string.Format(TableManager.Instance.stringTable.GetText(1001), textInputNick);
                UserInfo userInfo = new UserInfo(textInputNick.text);
                okAction = () => SaveData_Local.Instance.SetUserInfo(userInfo); SaveData_PlayerSkill.Instance.SetSkillInfo(); Close();
                break;
            case eNickDeny.empty:
                title = TableManager.Instance.stringTable.GetText(1000);
                dest = TableManager.Instance.stringTable.GetText(1002);
                break;
            case eNickDeny.maxLength:
                title = TableManager.Instance.stringTable.GetText(1000);
                dest = TableManager.Instance.stringTable.GetText(1003);
                break;
            case eNickDeny.noUseText:
                title = TableManager.Instance.stringTable.GetText(1000);
                dest = TableManager.Instance.stringTable.GetText(1004);
                break;
        }
        uiManager.OpenMessageBox_Ok(title, dest, okAction: okAction);
    }

    private void OnClickCancle()
    {

    }
    #endregion

    private void CheckNickName(string name)
    {
        if (string.IsNullOrEmpty(name)) { nickState = eNickDeny.empty; return; };
        if (name.Length > Define.MaxNickName) {nickState = eNickDeny.maxLength ; return; };
        if(IsVaildString(name) == false) { nickState = eNickDeny.noUseText; return; };

        nickState = eNickDeny.none;
        return;
    }

    private bool IsVaildString(string name)
    {
        string pattern = @"[a-zA-Z0-9°¡-ÆR]$";
        return System.Text.RegularExpressions.Regex.IsMatch(name, pattern);
    }
}
