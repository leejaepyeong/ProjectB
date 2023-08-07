using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tween_Alpha : Tween<float>
{
    private Image img;
    Color color;

    protected override void Setting()
    {
        base.Setting();
        img = GetComponent<Image>();

        if (img == null)
            return;

        color = img.color;
        color.a = from;
        img.color = color;
    }

    protected override void Active(float percent)
    {
        base.Active(percent);
        if (img == null)
            return;

        color.a = from + (to - from) * percent;
        img.color = color;
    }
}
