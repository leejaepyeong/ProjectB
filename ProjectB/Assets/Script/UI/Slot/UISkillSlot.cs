using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class SkillInfo
{
    private float elaspedTIme;
    public float coolTime;
    public SkillRecord skillRecord;
    public Dictionary<int, RuneRecord> runeDic = new Dictionary<int, RuneRecord>();
    public List<SkillEffectRecord> skillEffectList = new List<SkillEffectRecord>();

    public SkillInfo(SkillRecord skill)
    {
        skillRecord = skill;
        ReSetSkillInfo();
    }
    public SkillInfo(int slotIdx)
    {
        skillRecord = null;
        if (Manager.Instance.CurScene.isTestScene)
        {
            if(TableManager.Instance.skillTable.GetRecord(Manager.Instance.playerData.equipSkill[slotIdx]) != null)
                skillRecord = TableManager.Instance.skillTable.GetRecord(Manager.Instance.playerData.equipSkill[slotIdx]);
        }
        else
        {
            var playerSkill = SaveData_PlayerSkill.Instance.GetEquipSkill(slotIdx);
            if (playerSkill == null) return;
            skillRecord = playerSkill.GetSkillData();
        }
        ReSetSkillInfo();

        runeDic.Clear();
        for (int i = 0; i < Define.MaxEquipRune; i++)
        {
            runeDic.Add(i, null);
        }
    }

    public void ReSetSkillInfo()
    {
        if (skillRecord == null)
        {
            Debug.LogError("Skill Record is Null");
            return;
        }

        skillEffectList.Clear();
        for (int i = 0; i < skillRecord.skillEffects.Count; i++)
        {
            if (TableManager.Instance.skillEffectTable.TryGetRecord(skillRecord.skillEffects[i], out var skillEffect) == false)
                continue;
            skillEffectList.Add(skillEffect);
        }

        coolTime = skillRecord.coolTIme;
        SetCoolTime();
        skillRecord.skillNode.SetSkill(this);
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
        runeDic[equipIdx] = runeRecord;
        ReSetSkillInfo();
    }

    public void RemoveRune(int runeIdx)
    {
        runeDic[runeIdx] = null;
        ReSetSkillInfo();
    }

    public void ChangeSkill(SkillRecord skill)
    {
        skillRecord = skill;
        ReSetSkillInfo();
    }

    public void UseSkill()
    {
        UnitManager.Instance.Player.Action(skillRecord.skillNode);
        SetCoolTime();
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
        if(isPlacement)
        {
            if (isMainSkillSlot)
                onClickAction = OnClickMainSkill_Placement;
            else
                onClickAction = OnClickActiveSkill_Placement;
        }
        else
            onClickAction = OnClickSkill;
        base.Awake();
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
        SetIcon(skillIcon, skillInfo.skillRecord != null ? skillInfo.skillRecord.iconPath : "");
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isPlacement) return;

        skillInfo.Update(deltaTime);
        SetCoolTime();
    }

    public void SetCoolTime()
    {
        blockIcon.gameObject.SetActive(skillInfo.IsReadySkill() == false);
        if (skillInfo.getTime == 0)
        {
            textCoolTime.SetText("");
            blockIcon.fillAmount = 0;
        }
        else
        {
            textCoolTime.SetText(skillInfo.getTime.ToString("F1"));
            blockIcon.fillAmount = skillInfo.getTime/ skillInfo.coolTime;
        }
    }

    #region Button Click
    protected void OnClickSkill()
    {
        if (isMainSkillSlot == false) return;
        if (skillInfo.skillRecord == null) return;
        if (skillInfo.skillRecord.type != eSkillType.Active) return;

        if(skillInfo.skillRecord.targetType == eSkillTarget.Target)
        {
            PlayLogic.Instance.UseTargetSkill(skillInfo);
        }
        else if(skillInfo.skillRecord.targetType == eSkillTarget.Target_Direction)
        {
            PlayLogic.Instance.UseTargetSkill(skillInfo);
        }
        else
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
