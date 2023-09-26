using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class RuneInfo
{

}
public class SkillInfo
{
    private float elaspedTIme;
    private float coolTime;
    private Data.SkillInfoData skillData;
    public Data.SkillInfoData SkillData => skillData;
    public List<Data.RuneInfoData> runeDataList = new List<Data.RuneInfoData>();
    public List<Data.SkillEffectInfo> skillEffectList = new List<Data.SkillEffectInfo>();

    public SkillInfo(int slotIdx)
    {
        elaspedTIme = 0;
        var playerSkill = SaveData_PlayerSkill.Instance.GetEquipSkill(slotIdx);
        skillData = playerSkill.GetSkillData();
        runeDataList.Clear();
        for (int i = 0; i < playerSkill.equipRuneGroup.Length; i++)
        {
            if (playerSkill.equipRuneGroup[i] == 0) continue;
            runeDataList.Add(playerSkill.GetRuneData(playerSkill.equipRuneGroup[i]));
        }
        skillEffectList.Clear();
        SetRuneEffect();
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

    private void SetRuneEffect()
    {

        for (int i = 0; i < runeDataList.Count; i++)
        {
            runeDataList[i].AddSkillEffectToSkill(skillEffectList);
        }

        for (int i = 0; i < skillEffectList.Count; i++)
        {
            runeDataList[i].SetRuneEffectToSkill(skillData, skillEffectList[i]);
        }
    }

    public void UseSkill()
    {
        Manager.Instance.skillManager.UseSkill(UnitManager.Instance.Player, this);
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
    private int slotIndex;

    public virtual void Init(int index)
    {
        buttonClick.onClick.AddListener(OnClickSkill);
        slotIndex = index;
        skillInfo = new SkillInfo(slotIndex);
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
        if (skillInfo.SkillData.type != eSkillType.Active) return;
        skillInfo.UseSkill();
    }
    #endregion
}
