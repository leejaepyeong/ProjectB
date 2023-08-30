using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;

public class UIMessageBox : UIBase
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

    private bool isCancleOn;

    private UnityAction okAction;
    private UnityAction cancleAction;

    public class UIParameter : Parameter
    {
        public string title;
        public string dest;
        public string ok = "1";
        public string cancle = "2";
        public bool isCancleOn = true;
        public bool escapable = true;
        public bool backable = true;
        public UnityAction okAction;
        public UnityAction cancleAction;
    }

    public override void SetParam(Parameter param)
    {
        if(param is UIParameter parameter)
        {
            title = parameter.title;
            dest = parameter.dest;
            ok = parameter.ok;
            cancle = parameter.cancle;
            escapable = parameter.escapable;
            backable = parameter.backable;
            isCancleOn = parameter.isCancleOn;
            okAction = parameter.okAction;
            cancleAction = parameter.cancleAction;
        }
    }

    public override void Init()
    {
        base.Init();
        buttonOk.onClick.AddListener(OnClickOk);
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
        SetText(textOk, ok);
        SetText(textCancle, cancle);
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
