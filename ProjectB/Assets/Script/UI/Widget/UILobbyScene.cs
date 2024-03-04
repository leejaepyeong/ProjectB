using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UILobbyScene : UIBase
{
    [SerializeField, FoldoutGroup("Top_Right")] private Button buttonOption;
    [SerializeField, FoldoutGroup("Bottom_Right")] private GameObject objButtonGroup;
    [SerializeField, FoldoutGroup("Bottom_Right")] private Button buttonStart;
    [SerializeField, FoldoutGroup("Bottom_Right")] private Button buttonInven;
    [SerializeField, FoldoutGroup("Bottom_Right")] private Button buttonAccount;

    private void Awake()
    {
        
    }

    public override void Open()
    {
        base.Open();
    }

    public override void ResetData()
    {
    }

    #region Button Click
    private void OnClickStart()
    {

    }
    private void OnClickInven()
    {

    }
    private void OnClickAccount()
    {

    }
    #endregion
}
