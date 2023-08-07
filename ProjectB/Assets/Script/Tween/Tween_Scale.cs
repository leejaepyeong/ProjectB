using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween_Scale : Tween<Vector3>
{
    protected override void Setting()
    {
        base.Setting();
        transform.localScale = from;
    }

    protected override void Active(float percent)
    {
        base.Active(percent);
        transform.localScale = from + (to - from) * percent;
    }

}
