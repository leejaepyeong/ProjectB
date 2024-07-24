using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo
{
    private float elaspedTIme;
    public float coolTime;
    public SkillRecord skillRecord;
    public Dictionary<int, RuneRecord> runeDic = new Dictionary<int, RuneRecord>();

    public Vector3 targetPos;

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
            if (TableManager.Instance.skillTable.GetRecord(Manager.Instance.playerData.equipSkill[slotIdx]) != null)
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
    public SkillInfo(int slotIdx, bool isPassive)
    {

    }

    public void ReSetSkillInfo()
    {
        if (skillRecord == null)
        {
            Debug.LogError("Skill Record is Null");
            return;
        }

        coolTime = skillRecord.coolTIme;
        SetCoolTime();
        skillRecord.skillNode.SetSkill(this);
    }

    public float getTime { get { return elaspedTIme; } }

    public void Update(float deltaTime)
    {
        if (skillRecord == null) return;
        if (IsReadySkill())
        {
            if (skillRecord.type == eSkillType.Auto && 
                Manager.Instance.skillManager.GetTargetList(BattleManager.Instance.player, skillRecord).Count > 0)
                UseSkill();
            return;
        }

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
        SkillManager.Instance.SpawnSkill(this, UnitManager.Instance.Player);
        SetCoolTime();
    }
}
public class PlayerData
{
    public long curExp { get; private set; }
    public long needExp { get; private set; }
    public int curLv { get; private set; }
    private ExpRecord curExpRecord;

    private int preLv;
    public long killCount { get; private set; }

    public List<InvenItemInfo> invenItemList = new List<InvenItemInfo>();
    private Dictionary<int, InvenItemInfo> dicItem = new Dictionary<int, InvenItemInfo>();
    private int itemId;

    public List<SkillInfo> mainSkillInfoList = new List<SkillInfo>(mainSkillCount);
    public List<SkillInfo> activeSkillInfoList = new List<SkillInfo>(activeSkillCount);
    public List<SkillInfo> passiveSkillInfoList = new List<SkillInfo>(passiveSkillCount);
    private const int mainSkillCount = 5;
    private const int activeSkillCount = 5;
    private const int passiveSkillCount = 5;

    public void Init()
    {
        curExp = 0;
        curLv = 1;
        killCount = 0;
        itemId = 0;
        invenItemList.Clear();

        mainSkillInfoList.Clear();
        activeSkillInfoList.Clear();
        passiveSkillInfoList.Clear();

        SetMainSkill();
        SetSubSkill();
    }

    private void SetMainSkill()
    {
        for (int i = 0; i < mainSkillCount; i++)
        {
            SkillInfo skill = new SkillInfo(i);
            mainSkillInfoList.Add(skill);
        }
    }
    private void SetSubSkill()
    {
        for (int i = 0; i < activeSkillCount; i++)
        {
            SkillInfo skill = new SkillInfo(i, false);
            activeSkillInfoList.Add(skill);
        }
        for (int i = 0; i < passiveSkillCount; i++)
        {
            SkillInfo skill = new SkillInfo(i, true);
            passiveSkillInfoList.Add(skill);
        }
    }

    public void AddExp(long exp)
    {
        curExp += exp;
        if (curExp >= needExp)
        {
            preLv = curLv;
            LevelUp();
        }
        PlayLogic.Instance.uiPlayLogic.UpdateExp();
    }

    public void LevelUp()
    {
        var expRecord = TableManager.Instance.expTable.GetExpRecord(curExp, out long remainExp);
        if (expRecord == null) return;

        curLv = expRecord.level;
        curExp = remainExp;
        needExp = expRecord.needExp;
        UILevelUpDlg dlg = UIManager.Instance.OpenWidget<UILevelUpDlg>();
        dlg.Open(curLv - preLv);
    }
    public void AddKillCount()
    {
        killCount += 1;
    }

    #region Item
    public void AddISkilltem(int skillIdx)
    {
        itemId += 1;
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, skillIdx, eItemType.Skill);
        invenItemList.Add(itemInfo);
        dicItem.Add(itemId, itemInfo);
    }

    public void AddRuneItem(int runeIdx)
    {
        itemId += 1;
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, runeIdx, eItemType.Rune);
        invenItemList.Add(itemInfo);
        dicItem.Add(itemId, itemInfo);
    }
    public void AddUseItem(int useIndex)
    {
        itemId += 1;
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, useIndex, eItemType.Use);
        invenItemList.Add(itemInfo);
        dicItem.Add(itemId, itemInfo);
    }

    public void RemoveItem(int itemId)
    {
        if (dicItem.TryGetValue(itemId, out var item) == false) return;

        invenItemList.Remove(item);
        item = null;
    }
    #endregion

    #region Equip Skill & Rune
    public void EquipRune(int slotIndex, int runeSlotIndex, RuneRecord runeRecord)
    {
        mainSkillInfoList[slotIndex].runeDic[runeSlotIndex] = runeRecord;
        ResetInfo();
    }
    public void EquipActiveSkill(int slotIndex, SkillRecord skillRecord)
    {
        activeSkillInfoList[slotIndex].ChangeSkill(skillRecord);
        ResetInfo();
    }
    public void EquipPassiveSkill(int slotIndex, SkillRecord skillRecord)
    {
        passiveSkillInfoList[slotIndex].ChangeSkill(skillRecord);
        ResetInfo();
    }
    public void ResetInfo()
    {
        PlayLogic.Instance.uiPlayLogic.uiSkillInven.ResetData();
    }
    #endregion
}
