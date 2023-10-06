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
    public SkillRecord skillRecord;
    public List<RuneRecord> runeList = new List<RuneRecord>();
    public List<SkillEffectRecord> skillEffectList = new List<SkillEffectRecord>();

    public SkillInfo(SkillRecord skill)
    {
        skillRecord = skill.GetCopyRecord();
        skillEffectList.Clear();
        for (int i = 0; i < skillRecord.skillEffects.Length; i++)
        {
            if (TableManager.Instance.skillEffectTable.TryGetRecord(skillRecord.skillEffects[i], out var skillEffect) == false)
                continue;
            var copy = skillEffect.GetCopyRecord();
            skillEffectList.Add(copy);
        }
    }
    public SkillInfo(int slotIdx)
    {
        elaspedTIme = 0;
        var playerSkill = SaveData_PlayerSkill.Instance.GetEquipSkill(slotIdx);
        skillRecord = playerSkill.GetSkillData();
        skillEffectList.Clear();
        for (int i = 0; i < skillRecord.skillEffects.Length; i++)
        {
            if (TableManager.Instance.skillEffectTable.TryGetRecord(skillRecord.skillEffects[i], out var skillEffect) == false)
                continue;
            var copy = skillEffect.GetCopyRecord();
            skillEffectList.Add(copy);
        }
        runeList.Clear();
        for (int i = 0; i < playerSkill.equipRuneGroup.Length; i++)
        {
            if (playerSkill.equipRuneGroup[i] == 0) continue;
            runeList.Add(playerSkill.GetRuneData(playerSkill.equipRuneGroup[i]));
        }
        
        SetRuneEffect();
    }

    public float getTime { get { return elaspedTIme; } }

    public void Update(float deltaTime)
    {
        if (IsReadySkill()) return;

        elaspedTIme -= deltaTime;
        if (elaspedTIme < 0) elaspedTIme = 0;
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

        for (int i = 0; i < runeList.Count; i++)
        {
            runeList[i].AddSkillEffectToSkill(skillEffectList);
        }

        for (int i = 0; i < runeList.Count; i++)
        {
            runeList[i].SetRuneEffectToSkill(skillRecord);
            for (int j = 0; j < skillEffectList.Count; j++)
            {
                runeList[i].SetRuneEffectToSkillEffect(skillEffectList[j]);
            }
        }
    }

    public void UseSkill()
    {
        var targetList = Manager.Instance.skillManager.GetTargetList(UnitManager.Instance.Player, skillRecord);
        UnitManager.Instance.Player.SetTargets(targetList);
        UnitManager.Instance.Player.isUseSkill = true;
        UnitManager.Instance.Player.skillInfo = this;
        UnitManager.Instance.Player.Action(skillRecord.skillNode);
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
        if (skillInfo.getTime == 0)
            textCoolTime.SetText("");
        else
            textCoolTime.SetText(skillInfo.getTime.ToString("F1"));
    }

    #region Button Click
    protected void OnClickSkill()
    {
        if (skillInfo.skillRecord.type != eSkillType.Active) return;
        if (UnitManager.Instance.Player.isUseSkill) return;

        skillInfo.UseSkill();
    }
    #endregion
}
