using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UISkillSlot : UISlot
{
    [SerializeField, FoldoutGroup("Info")] protected Image skillIcon;
    [SerializeField, FoldoutGroup("Info")] protected Image OutLineIcon;
    [SerializeField, FoldoutGroup("Info")] protected int slotIdx;
    [SerializeField, FoldoutGroup("Info")] protected bool isMainSkillSlot;
    [SerializeField, FoldoutGroup("Info")] protected bool isPlacement;
    [SerializeField, FoldoutGroup("Info")] protected UISkillInven_Placement uiSkillInvenPlacement;

    [SerializeField, FoldoutGroup("Block")] protected Image blockIcon;
    [SerializeField, FoldoutGroup("Block")] protected TextMeshProUGUI textCoolTime;

    protected SkillInfo skillInfo;
    protected int slotIndex;
    protected UIInvenItemSlot curInvenItemSlot;

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
        skillInfo = BattleManager.Instance.playerData.skillInfoList[index];
    }

    public virtual void Open()
    {
        base.Open();
        ResetData();
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

        if(skillInfo.skillRecord.targetType == eSkillTarget.Click_Target)
        {
            PlayLogic.Instance.UseTargetSkill(skillInfo);
        }
        else if(skillInfo.skillRecord.targetType == eSkillTarget.Click_Direction)
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
        curInvenItemSlot.Equip(this);
        skillInfo = new SkillInfo(curInvenItemSlot.getSkillRecord);
        ResetData();
    }

    protected void OnClickMainSkill_Placement()
    {
        var dlg = uiManager.OpenWidget<UIRuneChangeDlg>();
        dlg.Open(this ,uiSkillInvenPlacement.selectSlot);
    }

    public void OnClickUnEquip()
    {
        curInvenItemSlot = null;
        skillInfo.skillRecord = null;
        ResetData();
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
