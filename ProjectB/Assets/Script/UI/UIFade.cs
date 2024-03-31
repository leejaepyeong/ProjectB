using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class UIFade : UIDlg
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

    public async UniTask OpenAsync(UniTask actionAsync)
    {
        this.actionAsync = actionAsync;
        isAsync = true;
        fadeImg.color = new Color(0, 0, 0, 0);
        fadeImg.raycastTarget = true;
        isFadeAct = true;
        await FadeAsync();
    }

    private async UniTask FadeAsync()
    {
        await FadeInAsync();

        if (isAsync)
        {
            await actionAsync;
        }
        else
        {
            action?.Invoke();
        }
        //await UniTask.Delay(TimeSpan.FromSeconds(1));

        await FadeOutAsync();
        
        isFadeAct = false;
        fadeImg.raycastTarget = false;
        Close();
    }

    private async UniTask FadeInAsync()
    {
        float time = 0;
        while (time <= 1)
        {
            fadeImg.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time));
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        fadeImg.color = new Color(0, 0, 0, 1);
    }

    private async UniTask FadeOutAsync()
    {
        float time = 0;
        while (time <= 1)
        {
            fadeImg.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time));
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        fadeImg.color = new Color(0, 0, 0, 0);
    }
}
