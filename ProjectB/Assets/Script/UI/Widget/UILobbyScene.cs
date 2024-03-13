using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class UILobbyScene : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField, FoldoutGroup("Top_Right")] private Button buttonOption;
    [SerializeField, FoldoutGroup("Bottom_Right")] private GameObject objButtonGroup;
    [SerializeField, FoldoutGroup("Bottom_Right")] private Button buttonStart;
    [SerializeField, FoldoutGroup("Bottom_Right")] private Button buttonInven;
    [SerializeField, FoldoutGroup("Bottom_Right")] private Button buttonAccount;

    private bool interactive;

    private void Awake()
    {
        canvas.worldCamera = UIManager.Instance.UiCamera;
        buttonOption.onClick.AddListener(OnClickOption);
        buttonStart.onClick.AddListener(OnClickStart);
        buttonInven.onClick.AddListener(OnClickInven);
        buttonAccount.onClick.AddListener(OnClickAccount);
    }

    public void Open()
    {
        interactive = true;
        ResetData();
    }

    public void ResetData()
    {
    }

    #region Button Click
    private void OnClickStart()
    {
        if (interactive == false) return;
        interactive = false;

        if (Manager.Instance.CurScene is LobbyScene lobbyScene)
        {
            lobbyScene.SetCommand(new LobbyCommand_MoveToPlayScene(null));
        }
    }
    private void OnClickInven()
    {
        if (interactive == false) return;

    }
    private void OnClickAccount()
    {
        if (interactive == false) return;

    }
    private void OnClickOption()
    {
        if (interactive == false) return;

    }
    #endregion
}
