using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseScene : MonoBase
{
    [SerializeField, FoldoutGroup("User")] public bool isTestScene;
    [SerializeField, FoldoutGroup("User")] public TestPlayerData playerData;
    private void Awake()
    {
        Manager.Instance.SetUI(this);
        Init();
    }

    public virtual void Init()
    {

    }
}
