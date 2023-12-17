using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIBase : MonoBase
{
    public Button btnClick;
    public Button[] btnCloseGroup;
    public RectTransform RectTransform => transform as RectTransform;
    protected UIManager uiManager => UIManager.Instance;

    protected bool escapable;
    protected bool backable;
    protected UnityAction onClickAction;

    protected virtual void Awake()
    {
        if(btnClick != null)
            btnClick.onClick.AddListener(onClickAction);
        for (int i = 0; i < btnCloseGroup.Length; i++)
        {
            btnCloseGroup[i].onClick.AddListener(Close);
        }
    }

    public virtual void DeInit()
    {

    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        Manager.Instance.GetManager<UIManager>().RemoveUI(this);
    }

    public override void UpdateFrame(float deltaTime)
    {

    }

    public virtual void OnClickBackGround()
    {
        Close();
    }

    public virtual void OnClickEscape()
    {
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
