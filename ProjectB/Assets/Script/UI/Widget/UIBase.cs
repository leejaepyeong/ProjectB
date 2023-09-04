using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBase : MonoBase
{
    public RectTransform RectTransform => transform as RectTransform;
    protected UIManager uiManager => UIManager.Instance;

    protected bool escapable;
    protected bool backable;

    public virtual void SetParam(Parameter param)
    {

    }

    public override void Init()
    {
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
