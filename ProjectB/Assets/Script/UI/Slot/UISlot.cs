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


