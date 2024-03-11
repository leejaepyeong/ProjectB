using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class UIFade : UIBase
{
    [SerializeField] private Image fadeImg;
    protected UnityAction action;
    protected UniTask actionAsync;
    private bool isFadeAct;
    private bool isAsync;
    public bool IsFadeAct => isFadeAct;

    public virtual void Open(UnityAction action = null)
    {
        base.Open();
        this.action = action;
        isAsync = false;
        ResetData();
    }

    public virtual void Open(UniTask actionAsync)
    {
        base.Open();
        this.actionAsync = actionAsync;
        isAsync = true;
        ResetData();
    }

    public override void ResetData()
    {
        fadeImg.color = new Color(0, 0, 0, 0);
        fadeImg.raycastTarget = true;
        isFadeAct = true;
        FadeAsync().Forget();
    }

    private async UniTask FadeAsync()
    {
        float time = 0;
        while (time <= 1)
        {
            fadeImg.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time));
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        fadeImg.color = new Color(0, 0, 0, 1);

        if (isAsync)
        {
            await actionAsync;
        }
        else
        {
            action?.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }

        time = 0;
        while (time <= 1)
        {
            fadeImg.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time));
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        fadeImg.color = new Color(0, 0, 0, 0);
        isFadeAct = false;
        fadeImg.raycastTarget = false;
        Close();
    }
}
