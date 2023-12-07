using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UISkillInven : UIBase
{
    [FoldoutGroup("Info")] public UISkillGroup mainSkillGroup;
    [FoldoutGroup("Info")] public SkillInven activeSkillInven;
    [FoldoutGroup("Info")] public SkillInven passiveSkillInven;

    public virtual void Init()
    {
        mainSkillGroup.Init();
    }
    public override void Open()
    {
        base.Open();
        ResetData();
    }

    public override void ResetData()
    {
        base.ResetData();
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
    }
}
