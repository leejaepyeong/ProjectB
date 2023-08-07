using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mosframe;

public class UISlot : UIBehaviour, IDynamicScrollViewItem
{
    private int index;
    public RectTransform RectTransform => transform as RectTransform;

    public virtual void Init()
    {
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

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public void onUpdateItem(int index)
    {
        this.index = index;
    }
}


