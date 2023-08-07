using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mosframe;

public class UITest : UIBase
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textDest;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button scrollTestBtn;

    [SerializeField] private DynamicVScrollView scrollView;
    [SerializeField] private UIGroupSlot groupSlot;

    [SerializeField] private float totalCnt =  20;
    private string titleName;
    private string dest;

    public class UIParameter : Parameter
    {
        public string titleName;
        public string dest;
    }

    public override void SetParam(Parameter param)
    {
        if (param is UIParameter parameter)
        {
            titleName = parameter.titleName;
            dest = parameter.dest;
        }
    }

    public override void Init()
    {
        backBtn.onClick.AddListener(OnClickEscape);
        scrollTestBtn.onClick.AddListener(SetScrollView);
        base.Init();
    }

    public override void Open()
    {
        textTitle.SetText(titleName);
        textDest.SetText(dest);
        SetScrollView();
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void DeInit()
    {
        backBtn.onClick.RemoveAllListeners();
        scrollTestBtn.onClick.RemoveAllListeners();
    }

    private void SetScrollView()
    {
        scrollView.totalItemCount = Mathf.CeilToInt(totalCnt / groupSlot.ConstraintCount);
        scrollView.refresh();
    }
}
