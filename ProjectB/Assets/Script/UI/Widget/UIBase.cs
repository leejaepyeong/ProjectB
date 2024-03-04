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
    [SerializeField, FoldoutGroup("Setting")] protected bool backable;
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
