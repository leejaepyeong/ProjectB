using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UISkillInven : UIBase
{
    [SerializeField, FoldoutGroup("Info")] UISkillSlot[] activeSkillGroup;
    [SerializeField, FoldoutGroup("Info")] UISkillSlot[] passiveGroup;

    public override void Open()
    {
        base.Open();
        ResetData();
    }

    public override void ResetData()
    {
        base.ResetData();
    }
}
