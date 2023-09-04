using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class SkillInfo
{
    private float elaspedTIme;
    private float coolTime;

    public SkillInfo(float cool)
    {
        elaspedTIme = 0;
        coolTime = cool;
    }

    public float getTime { get { return elaspedTIme; } }

    public void Update(float deltaTime)
    {
        if (IsReadySkill()) return;

        elaspedTIme -= deltaTime;
    }

    public bool IsReadySkill()
    {
        return elaspedTIme <= 0;
    }

    public void SetCoolTime()
    {
        elaspedTIme = coolTime;
    }
}

public class UISkillSlot : UISlot
{
    [SerializeField] private Button buttonClick;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image OutLineIcon;
    [SerializeField, FoldoutGroup("Block")] private Image blockIcon;
    [SerializeField, FoldoutGroup("Block")] private TextMeshProUGUI textCoolTime;

    private SkillInfo skillInfo;

    public override void Init()
    {
        buttonClick.onClick.AddListener(OnClickSkill);
        skillInfo = new SkillInfo(10f);
    }

    public override void UnInit()
    {
        buttonClick.onClick.RemoveAllListeners();
    }

    public virtual void Open()
    {
        SetIcon(skillIcon, "");
    }

    public override void UpdateFrame(float deltaTime)
    {
        skillInfo.Update(deltaTime);
        SetCoolTime();
    }

    public void SetCoolTime()
    {
        textCoolTime.SetText(skillInfo.getTime.ToString("F1"));
    }

    #region Button Click
    private void OnClickSkill()
    {

    }
    #endregion
}
