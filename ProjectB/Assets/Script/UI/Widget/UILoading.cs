using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class UILoading : UIBase
{
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textGuage;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textDest;

    private float guage;
    private float maxGuage;

    public class UIParameter : Parameter
    {
    }

    public override void SetParam(Parameter param)
    {

    }

    public override void Init()
    {
        base.Init();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public bool CheckLoadinState()
    {
        return false;
    }
}
