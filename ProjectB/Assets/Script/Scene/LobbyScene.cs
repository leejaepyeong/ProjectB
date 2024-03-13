using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    [SerializeField] private UILobbyScene uiLobbyScene;
    #region Commnad
    public QueueCommand commands = new QueueCommand();
    #endregion
    public override void Init()
    {
        OpenUI();
    }

    public override void UpdateFrame(float deltaTime)
    {
        commands.UpdateFrame(deltaTime);
    }

    private void OpenUI()
    {
        uiLobbyScene.Open();
    }

    public void SetCommand(Command command)
    {
        commands.Add(command);
    }
}
