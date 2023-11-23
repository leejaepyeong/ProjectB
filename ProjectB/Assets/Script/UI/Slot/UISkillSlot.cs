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
        skillRecord = null;

        if (Manager.Instance.CurScene.isTestScene)
        {
            skillRecord = TableManager.Instance.skillTable.GetRecord(Manager.Instance.playerData.equipSkill[slotIdx]);
        }
        else
        {
            var playerSkill = SaveData_PlayerSkill.Instance.GetEquipSkill(slotIdx);
            if (playerSkill == null) return;
            skillRecord = playerSkill.GetSkillData();
        }

        skillEffectList.Clear();
        for (int i = 0; i < skillRecord.skillEffects.Length; i++)
        {
            if (TableManager.Instance.skillEffectTable.TryGetRecord(skillRecord.skillEffects[i], out var skillEffect) == false)
                continue;
            var copy = skillEffect.GetCopyRecord();
            skillEffectList.Add(copy);
        }
        runeList.Clear();
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

    public void AddRune(RuneRecord runeRecord, int equipIdx)
    {
        var copy = runeRecord.GetCopyRecord();
        copy.AddSkillEffectToSkill(skillEffectList);
        copy.SetRuneEffectToSkill(skillRecord);
        for (int j = 0; j < skillEffectList.Count; j++)
        {
            copy.SetRuneEffectToSkillEffect(skillEffectList[j]);
        }

        if (equipIdx == -1)
        {
            runeList.Add(copy);
        }
        else
        {
            runeList[equipIdx] = copy;
        }
    }

    public void RemoveRune(int runeIdx)
    {
        runeList[runeIdx] = null;
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

    protected override void Awake()
    {
        base.Awake();
        buttonClick.onClick.AddListener(OnClickSkill);
    }

    public void Init(int index)
    {
        slotIndex = index;
        skillInfo = new SkillInfo(slotIndex);
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
        if (skillInfo.skillRecord == null) return;
        if (skillInfo.skillRecord.type != eSkillType.Active) return;
        if (UnitManager.Instance.Player.isUseSkill) return;

        skillInfo.UseSkill();
    }
    #endregion

    public void ChangeRune(RuneRecord runeRecord, int equipIdx)
    {
        UnEquipRune(equipIdx);
        EquipRune(runeRecord, equipIdx);
    }

    public void EquipRune(RuneRecord runeRecord, int equipIdx = -1)
    {
        skillInfo.AddRune(runeRecord, equipIdx);
    }

    public void UnEquipRune(int equipIdx)
    {
        skillInfo.RemoveRune(equipIdx);
    }
}
