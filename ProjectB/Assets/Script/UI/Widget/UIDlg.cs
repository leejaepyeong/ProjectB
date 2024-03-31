using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class UIDlg : UIBase
{
    [SerializeField, FoldoutGroup("Setting")] protected Button[] btnCloseGroup;
    [SerializeField, FoldoutGroup("Setting")] protected bool escapable;
    [SerializeField, FoldoutGroup("Setting")] protected bool backGroundable;

    protected bool interactive;

    public bool Escapable => escapable;
    public bool BackGroundAble => backGroundable;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < btnCloseGroup.Length; i++)
        {
            btnCloseGroup[i].onClick.AddListener(Close);
        }
    }

    public override void Open()
    {
        base.Open();
        interactive = true;
    }
    public override void Close()
    {
        UIManager.Instance.RemoveUI(this);
        base.Close();
    }

    public virtual void OnClickBackGround()
    {
        if (backGroundable == false) return;

        Close();
    }
    public virtual void OnClickEscape()
    {
        if (escapable == false) return;

        Close();
    }
}
