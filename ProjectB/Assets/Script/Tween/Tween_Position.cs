using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tween_Position : Tween<Vector3>
{
    protected override void Setting()
    {
        base.Setting();
        transform.localPosition = from;
    }

    protected override void Active(float percent)
    {
        transform.localPosition = from + (to - from) * percent;
    }
}
