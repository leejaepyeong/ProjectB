using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class UIRuneChangeDlg : UIDlg
{
    [SerializeField, FoldoutGroup("Info")] private List<UIRuneSlot> uiRuneSlotGroup;
    [SerializeField, FoldoutGroup("Info")] private Image equipRuneIcon;
    [SerializeField, FoldoutGroup("Info")] private TextMeshProUGUI textSkillName;
    [SerializeField, FoldoutGroup("Info")] private TextMeshProUGUI textSkillDest;
    [SerializeField, FoldoutGroup("Info")] private TextMeshProUGUI textEquipRuneName;
    [SerializeField, FoldoutGroup("Info")] private TextMeshProUGUI textEquipRuneDest;
    [SerializeField, FoldoutGroup("Info")] private TextMeshProUGUI textPreviewRuneEffect;

    private UISkillSlot uiSkillSlot;
    private UIInvenItemSlot invenSlot;

    private Dictionary<int, RuneRecord> tempRuneDic = new Dictionary<int, RuneRecord>();
    private int previewIndex = 0;
    private UnityAction closeAction;

    protected override void Awake()
    {
        onClickAction = EquipRune;
        base.Awake();
    }

    public virtual void Open(UISkillSlot skillSlot, UIInvenItemSlot invenSlot, UnityAction closeAction)
    {
        base.Open();
        uiSkillSlot = skillSlot;
        this.invenSlot = invenSlot;
        this.closeAction = closeAction;

        SetText(textSkillName, uiSkillSlot.SkillInfo.skillRecord.getName);
        SetText(textSkillDest, uiSkillSlot.SkillInfo.skillRecord.getDest);

        SetIcon(equipRuneIcon, invenSlot.getRuneRecord != null ? invenSlot.getRuneRecord.iconPath : "");
        SetText(textSkillName, invenSlot.getRuneRecord.getName);
        SetText(textSkillDest, invenSlot.getRuneRecord.getDest);

        tempRuneDic.Clear();
        for (int i = 0; i < uiRuneSlotGroup.Count; i++)
        {
            tempRuneDic.Add(i, uiSkillSlot.SkillInfo.runeDic[i]);
        }

        ResetData();
    }

    public override void Close()
    {
        previewIndex = 0;
        tempRuneDic.Clear();
        closeAction?.Invoke();
        base.Close();
    }

    public override void ResetData()
    {
        for (int i = 0; i < uiRuneSlotGroup.Count; i++)
        {
            uiRuneSlotGroup[i].Open(tempRuneDic[i], EquipPreviewRune);
        }
        ShowPreviewEffect();
    }

    private void EquipPreviewRune(int slotIdx)
    {
        previewIndex = slotIdx;

        for (int i = 0; i < uiRuneSlotGroup.Count; i++)
        {
            if (slotIdx == i)
                tempRuneDic[slotIdx] = invenSlot.getRuneRecord;
            else
                tempRuneDic[i] = uiSkillSlot.SkillInfo.runeDic[i];
        }

        ResetData();
    }

    private void ShowPreviewEffect()
    {
        string previewText = "";
        for (int i = 0; i < uiRuneSlotGroup.Count; i++)
        {
            if (tempRuneDic[i] != null)
                previewText += $"{tempRuneDic[i].getDest}\n";
        }
        SetText(textSkillDest, invenSlot.getRuneRecord.getDest);
    }

    private void EquipRune()
    {
        BattleManager.Instance.playerData.EquipRune(uiSkillSlot.SlotIndex, previewIndex, invenSlot.getRuneRecord);
        Close();
    }
}
