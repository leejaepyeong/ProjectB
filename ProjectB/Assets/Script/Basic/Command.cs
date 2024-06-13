using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public interface Command
{
    void Execute();
    void Update(float deltaTime);
    bool IsFinished();
}

public class QueueCommand
{
    private Queue<Command> commandList = new Queue<Command>();
    private Command curCommand;

    public bool IsEmpty()
    {
        return null == commandList && commandList.Count <= 0;
    }

    public void Add(Command command)
    {
        if (command == null) return;
        commandList.Enqueue(command);
    }

    void Execute()
    {
        if (commandList.Count <= 0)
        {
            curCommand = null;
            return;
        }
        curCommand = commandList.Dequeue();
        curCommand.Execute();
    }

    public void UpdateFrame(float deltaTime)
    {
        if (curCommand == null && commandList.Count <= 0) return;

        if (curCommand != null)
        {
            curCommand.Update(deltaTime);
            if (curCommand.IsFinished())
                Execute();
        }
        else
            Execute();
    }

    public bool CommandEnd()
    {
        return commandList.Count == 0;
    }
}

#region Intro Command
public class IntroCommand_LoadDataFile : Command
{

    public void Execute()
    {
        Manager.Instance.getDataManager.ReadData();
    }

    public bool IsFinished()
    {
        return Data.DataManager.Instance.IsReadDone;
    }

    public void Update(float deltaTime)
    {
        if (IsFinished())
            Execute();
    }
}
public class IntroCommand_LoadLocalData : Command
{

    public void Execute()
    {
        Manager.Instance.getFileData.InitSaveData();
    }

    public bool IsFinished()
    {
        return FileData.Instance.IsLoadDone;
    }

    public void Update(float deltaTime)
    {
        if (IsFinished())
            Execute();
    }
}
public class IntroCommand_CreateAccount : Command
{
    private bool isComplete;
    private UICreateAccount uICreateAccount;
    public void Execute()
    {
        if (SaveData_Local.Instance.isSaveData)
        {
            isComplete = true;
            return;
        }

        uICreateAccount = UIManager.Instance.OpenWidget<UICreateAccount>();
        uICreateAccount.Open();
    }

    public bool IsFinished()
    {
        return isComplete;
    }

    public void Update(float deltaTime)
    {
        if (IsFinished())
            Execute();
    }
}
public class IntroCommand_MoveToLobby : Command
{
    private AsyncOperation loadSceneAsync;
    public void Execute()
    {
        Manager.Instance.Fade.OpenAsync(OpenLobbyAsync()).Forget();
    }

    public bool IsFinished()
    {
        return loadSceneAsync.isDone;
    }

    public void Update(float deltaTime)
    {
    }

    private async UniTask OpenLobbyAsync()
    {
        Manager.Instance.Loading.Open();

        loadSceneAsync = SceneManager.LoadSceneAsync("LobbyScene");
        await UniTask.WaitUntil(() => loadSceneAsync.isDone == false);
    }
}
#endregion

#region Lobby Command
public class LobbyCommand_MoveToPlayScene : Command
{
    private UnityEngine.Events.UnityAction moveAction;
    public LobbyCommand_MoveToPlayScene(UnityEngine.Events.UnityAction action)
    {
        moveAction = action;
    }
    private bool isSceneLoad;
    public void Execute()
    {
        isSceneLoad = false;
        Manager.Instance.Fade.Open(OpenPlaySceneAsync());
    }

    public bool IsFinished()
    {
        return isSceneLoad;
    }

    public void Update(float deltaTime)
    {
    }

    private async UniTask OpenPlaySceneAsync()
    {
        Manager.Instance.Loading.Open();

        await Manager.Instance.MoveSceneAsync("PlayScene");

        isSceneLoad = true;
    }
}
#endregion