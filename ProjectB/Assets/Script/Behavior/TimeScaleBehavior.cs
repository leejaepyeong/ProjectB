using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleBehavior
{
    private float timeScale = 1;
    public float TimeScale => timeScale;
    private float elaspedTime;
    private float duration;
    private AnimationCurve curve;

    public void Init(AnimationCurve curve)
    {
        this.curve = curve;
        duration = curve[curve.length - 1].time;
        elaspedTime = 0;
        timeScale = 1;
    }

    public void UpdateFrame(float deltaTime)
    {
        if (curve == null) return;
        elaspedTime += deltaTime;

        timeScale = Mathf.Clamp(elaspedTime > duration ? 1 : curve.Evaluate(elaspedTime), 0f, 3f);

        if (IsEnd)
            UnInit();
    }

    public void UnInit()
    {
        curve = null;
    }

    public bool IsEnd => elaspedTime >= duration;
}
