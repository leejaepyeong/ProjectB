using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;

public class UIPauseDlg : UIBase
{
    [FoldoutGroup("Center")]
    [SerializeField, FoldoutGroup("Center/GameSetting")] private Button buttonSound;
    [SerializeField, FoldoutGroup("Center/GameSetting")] private Button buttonGraphic;
    [SerializeField, FoldoutGroup("Center/GameSetting")] private Button buttonFsm;

    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private Scrollbar scrollAll;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private Scrollbar scrollBgm;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private Scrollbar scrollEffect;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private TextMeshProUGUI textAllValue;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private TextMeshProUGUI textBgmValue;
    [SerializeField, FoldoutGroup("Center/GameSetting/Sound")] private TextMeshProUGUI textEffectValue;

    [SerializeField, FoldoutGroup("Bottom")] private Button buttonRestart;
    [SerializeField, FoldoutGroup("Bottom")] private Button buttonBackGame;
    [SerializeField, FoldoutGroup("Bottom")] private Button buttonOutGame;

}
