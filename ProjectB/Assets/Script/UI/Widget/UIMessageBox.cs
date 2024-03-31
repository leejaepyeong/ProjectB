using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;

public class UIMessageBox : UIDlg
{
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textTitle;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textDest;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textOk;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textCancle;
    [SerializeField, FoldoutGroup("Center")] private Button buttonOk;
    [SerializeField, FoldoutGroup("Center")] private Button buttonCancle;

    private string title;
    private string dest;
    private string ok;
    private string cancle;

    private UnityAction okAction;
    private UnityAction cancleAction;

    protected override void Awake()
    {
        base.Awake();
        buttonOk.onClick.AddListener(OnClickOk);
        buttonCancle.onClick.AddListener(OnClickCancle);
    }

    public virtual void Open(string title, string dest, string okText, UnityAction okAction, string cancleText, UnityAction cancleAction)
    {
        base.Open();

        SetText(textTitle, title);
        SetText(textDest, dest);
        SetText(textOk, ok);
        SetText(textCancle, cancle);

        this.okAction = okAction;
        this.cancleAction = cancleAction;
    }

    #region Button Click
    private void OnClickOk()
    {
        if (okAction == null)
        {
            Close();
            return;
        }
        okAction.Invoke();
        Close();
    }

    private void OnClickCancle()
    {
        if (cancleAction == null)
        {
            Close();
            return;
        }
        cancleAction.Invoke();
        Close();
    }
    #endregion
}
