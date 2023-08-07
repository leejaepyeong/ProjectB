using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBase
{
    private void Awake()
    {
        Manager.Instance.SetUI(this);
        Init();
    }
}
