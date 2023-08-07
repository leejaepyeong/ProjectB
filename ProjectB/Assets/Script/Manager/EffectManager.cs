using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : BaseManager
{
    public static EffectManager Instance
    {
        get { return Manager.Instance.GetManager<EffectManager>(); }
    }

    public override void Init()
    {
        base.Init();
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
    }
}
