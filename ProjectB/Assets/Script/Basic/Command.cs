using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        uICreateAccount = UIManager.Instance.OpenUI<UICreateAccount>();
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
#endregion