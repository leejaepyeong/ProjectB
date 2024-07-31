using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UICreateAccount : UIDlg
{
    public enum eNickDeny
    {
        none,
        empty,
        maxLength,
        noUseText,
    }

    [SerializeField, FoldoutGroup("Center")] private TMP_InputField textInputNick;
    [SerializeField, FoldoutGroup("Center")] private GameObject objErrorMessage;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textErrorMessage;
    [SerializeField, FoldoutGroup("Center")] private Button buttonOk;

    private eNickDeny nickState;

    protected override void Awake()
    {
        base.Awake();
        buttonOk.onClick.AddListener(OnClickConfirm);
    }

    public override void Open()
    {
        base.Open();
        ResetData();
    }

    public override void ResetData()
    {
        base.ResetData();
        objErrorMessage.SetActive(false);
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
                objErrorMessage.SetActive(false);
                title = TableManager.Instance.stringTable.GetText(1000);
                dest = string.Format(TableManager.Instance.stringTable.GetText(1001), textInputNick);
                UserInfo userInfo = new UserInfo(textInputNick.text);
                okAction = () => SaveData_Local.Instance.SetUserInfo(userInfo); SaveData_PlayerSkill.Instance.SetSkillInfo(); Close();
                uiManager.OpenMessageBox_Ok(title, dest, okAction: okAction);
                return;
            case eNickDeny.empty:
                dest = TableManager.Instance.stringTable.GetText(1002);
                break;
            case eNickDeny.maxLength:
                dest = TableManager.Instance.stringTable.GetText(1003);
                break;
            case eNickDeny.noUseText:
                dest = TableManager.Instance.stringTable.GetText(1004);
                break;
        }
        objErrorMessage.SetActive(true);
        textErrorMessage.SetText(dest);
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
