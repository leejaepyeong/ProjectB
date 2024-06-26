using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIOptionDlg : UIDlg
{
    [SerializeField] private Button[] keyButtons;

    private int curChageKeyNum;

    public override void UpdateFrame(float deltaTime)
    {
        UpdateKeyOption();
    }

    #region KeySetting
    private void UpdateKeyOption()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey == false) return;

        SaveData_Option.Instance.ChangeKeySetting((eKey)curChageKeyNum, keyEvent.keyCode);
    }
    public void ChangeKey(int number)
    {
        curChageKeyNum = number;
    }
    #endregion
}
