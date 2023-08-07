using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween_Rotation : Tween<Vector3>
{
    protected override void Setting()
    {
        base.Setting();
        transform.rotation = Quaternion.Euler(from);
    }

    protected override void Active(float percent)
    {
        base.Active(percent);
        transform.rotation = Quaternion.Euler(from + (to - from) * percent);
    }
}
