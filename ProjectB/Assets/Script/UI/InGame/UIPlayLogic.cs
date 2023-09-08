using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIPlayLogic : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Top")] private UIWaveInfo uiWaveInfo; 

    public void Init()
    {

    }

    public void UpdateFrame(float deltaTime)
    {
        uiWaveInfo.UpdateFrame(deltaTime);
    }
}
