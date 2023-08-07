using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    public override void Init()
    {
        Manager.Instance.getFileData.InitSaveData();

        base.Init();
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (SaveData_Local.Instance.IsSaveData == false)
        {
            UIManager.Instance.OpenUI<UICreateAccount>();
            return;
        }


    }


}
