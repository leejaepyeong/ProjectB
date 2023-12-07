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
    public Dictionary<int, RuneRecord> runeDic = new Dictionary<int, RuneRecord>();
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
            skillRecord = TableManager.Instance.skillTable.GetRecord(Manager.Instance.playerData.equipSkill[slotIdx]).GetCopyRecord();
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
        runeDic.Clear();
        for (int i = 0; i < Define.MaxEquipRune; i++)
        {
            runeDic.Add(i, null);
        }
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

        runeDic[equipIdx] = copy;
    }

    public void RemoveRune(int runeIdx)
    {
        runeDic[runeIdx] = null;
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
    [SerializeField, FoldoutGroup("Info")] protected Image skillIcon;
    [SerializeField, FoldoutGroup("Info")] protected Image OutLineIcon;
    [SerializeField, FoldoutGroup("Info")] protected int slotIdx;
    [SerializeField, FoldoutGroup("Info")] protected bool isMainSkillSlot;
    [SerializeField, FoldoutGroup("Info")] protected bool isPlacement;

    [SerializeField, FoldoutGroup("Block")] protected Image blockIcon;
    [SerializeField, FoldoutGroup("Block")] protected TextMeshProUGUI textCoolTime;

    protected SkillInfo skillInfo;
    protected int slotIndex;
    protected UIInvenItemSlot curInvenItemSlot;
    protected UISkillInven_Placement uiSkillInvenPlacement;

    public SkillInfo SkillInfo => skillInfo;

    protected override void Awake()
    {
        base.Awake();
        if(isPlacement)
        {
            if (isMainSkillSlot)
                onClickAction = OnClickMainSkill_Placement;
            else
                onClickAction = OnClickActiveSkill_Placement;
        }
        else
            onClickAction = OnClickSkill;
    }

    public void Init(int index)
    {
        slotIndex = index;
        skillInfo = new SkillInfo(slotIndex);
    }

    public virtual void Open(UISkillInven_Placement uiSkillInvenPlacement = null)
    {
        base.Open();
        ResetData();
        this.uiSkillInvenPlacement = uiSkillInvenPlacement;
    }

    public override void ResetData()
    {
        SetIcon(skillIcon, skillInfo.skillRecord.iconPath);
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isPlacement) return;

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
        if (isMainSkillSlot == false) return;
        if (skillInfo.skillRecord == null) return;
        if (skillInfo.skillRecord.type != eSkillType.Active) return;
        if (UnitManager.Instance.Player.isUseSkill) return;

        skillInfo.UseSkill();
    }

    protected void OnClickActiveSkill_Placement()
    {
        if (curInvenItemSlot != null)
            curInvenItemSlot.UnEquip();

        curInvenItemSlot = uiSkillInvenPlacement.selectSlot;
        curInvenItemSlot.Equip();
        skillInfo = new SkillInfo(curInvenItemSlot.getSkillRecord);
        ResetData();
    }

    protected void OnClickMainSkill_Placement()
    {
        var dlg = uiManager.OpenWidget<UIRuneChangeDlg>();
        dlg.Open(this ,uiSkillInvenPlacement.selectSlot);
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
