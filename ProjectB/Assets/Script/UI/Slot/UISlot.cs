using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : UIBase
{
    private int index;

    protected override void Awake()
    {
        base.Awake();
        RectTransform.anchoredPosition3D = Vector3.zero;
        RectTransform.offsetMin = Vector2.zero;
        RectTransform.offsetMax = Vector2.zero;
        RectTransform.localRotation = Quaternion.identity;
        RectTransform.localScale = Vector3.one;
    }

    public virtual void Clear()
    { 
    }

    public virtual void Open(int index)
    {
        this.index = index;
        gameObject.SetActive(true);
    }

    public void onUpdateItem(int index)
    {
        this.index = index;
    }
}


