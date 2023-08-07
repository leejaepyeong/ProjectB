using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIFade : UIBase
{
    [SerializeField] private float fadeInValue = 1f;
    [SerializeField] private float fadeOutValue = 0f;
    protected UnityAction endAction;

    public virtual void FadeIn(UnityAction action)
    {
        Open();
        endAction = action;
    }

    public virtual void FadeOut(UnityAction action)
    {
        Open();
        endAction = action;
    }
}
