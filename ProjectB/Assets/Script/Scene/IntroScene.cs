using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IntroScene : BaseScene
{
    #region Commnad
    QueueCommand commands = new QueueCommand();
    IntroCommand_LoadDataFile loadDataFile = new IntroCommand_LoadDataFile();
    IntroCommand_LoadLocalData loadLocalData = new IntroCommand_LoadLocalData();
    IntroCommand_CreateAccount createAccount = new IntroCommand_CreateAccount();
    IntroCommand_MoveToLobby moveToLobby = new IntroCommand_MoveToLobby();
    #endregion
    public override void Init()
    {
        commands.Add(loadDataFile);
        commands.Add(loadLocalData);
        commands.Add(createAccount);
        commands.Add(moveToLobby);

        base.Init();
    }

    public override void UpdateFrame(float deltaTime)
    {
        commands.UpdateFrame(deltaTime);
    }
}
