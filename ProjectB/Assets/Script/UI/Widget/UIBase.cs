using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

public class UIBase : MonoBase
{
    public RectTransform RectTransform => transform as RectTransform;
    protected UIManager uiManager => UIManager.Instance;

    [SerializeField, FoldoutGroup("Setting")] protected Button btnClick;
    [SerializeField, FoldoutGroup("Setting")] protected Button[] btnCloseGroup;
    [SerializeField, FoldoutGroup("Setting")] protected bool escapable;
    [SerializeField, FoldoutGroup("Setting")] protected bool backGroundable;
    protected UnityAction onClickAction;
    protected bool interactive;

    public bool Escapable => escapable;
    public bool BackGroundAble => backGroundable;

    protected virtual void Awake()
    {
        if(btnClick != null)
            btnClick.onClick.AddListener(onClickAction);
        for (int i = 0; i < btnCloseGroup.Length; i++)
        {
            btnCloseGroup[i].onClick.AddListener(Close);
        }
    }

    public override void Open()
    {
        interactive = true;
        base.Open();
    }

    public override void Close()
    {
        UIManager.Instance.RemoveUI(this);
    }

    public override void UpdateFrame(float deltaTime)
    {

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

    public void SetText(TextMeshProUGUI textPro, string text)
    {
        textPro.gameObject.SetActive(string.IsNullOrEmpty(text) == false);
        textPro.SetText(text);
    }
    protected void SetIcon(Image img, string path)
    {
        if (img == null) return;
        if (string.IsNullOrEmpty(path)) return;

        img.sprite = Define.Load<Sprite>(path);
    }

}
