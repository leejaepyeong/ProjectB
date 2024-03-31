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
    protected UnityAction onClickAction;

    [SerializeField, FoldoutGroup("Setting")] protected Button btnClick;


    protected virtual void Awake()
    {
        if (btnClick != null)
            btnClick.onClick.AddListener(onClickAction);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void UpdateFrame(float deltaTime)
    {

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
