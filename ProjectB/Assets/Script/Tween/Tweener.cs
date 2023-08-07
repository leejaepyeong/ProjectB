using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Tweener : MonoBehaviour
{
    
    [SerializeField, FoldoutGroup("Setting")] protected float delay;
    [SerializeField, FoldoutGroup("Setting")] protected float duration;
    [SerializeField, FoldoutGroup("Setting")] protected AnimationCurve curve;

    protected bool isStart = true;
    protected bool isEnd = false;
    protected bool isReverse;

    private Coroutine coTween;

    protected virtual void Setting() { }

    protected virtual void Active(float percent) { }

    public virtual void Play() { }

    public virtual void ReversePlay() { }
}
