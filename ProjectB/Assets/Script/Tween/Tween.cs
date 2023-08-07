using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Tween<T> : Tweener
{
    private float curTime;

    [SerializeField, FoldoutGroup("Setting")] protected T from;
    [SerializeField, FoldoutGroup("Setting")] protected T to;
    [SerializeField, FoldoutGroup("Setting")] protected bool isLoop = false;

    private Coroutine coTween;

    public override void Play()
    {
        base.Play();
        isStart = true;
        this.enabled = true;

        if (coTween != null)
        {
            StopCoroutine(coTween);
            coTween = null;
        }
    }

    public override void ReversePlay()
    {
        base.ReversePlay();
        isReverse = true;

        var temp = from;
        from = to;
        to = temp;

        Play();
    }

    public void End()
    {
        isEnd = true;
        this.enabled = false;
    }


    public bool IsPlayTween()
    {
        if (isEnd)
            return false;

        return true;
    }

    protected void Update()
    {
        if(this.enabled && isStart)
        {
            isStart = false;
            if(coTween == null)
                coTween = StartCoroutine(coTweenPlay());
        }
    }

    protected IEnumerator coTweenPlay()
    {
        Setting();

        yield return new WaitForSeconds(delay);

        float curTime = 0;
        float curPercent;

        while(curTime <= duration)
        {
            curTime += Time.deltaTime;
            curPercent = curve.Evaluate(curTime / duration);

            if (curPercent > 1)
                curPercent = 1;
            Active(curPercent);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(isLoop)
        {
            StopCoroutine(coTween);
            coTween = StartCoroutine(coTweenPlay());
        }    

        if(isReverse)
        {
            isReverse = false;
            var temp = from;
            from = to;
            to = temp;
        }
    }
}
