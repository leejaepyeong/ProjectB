using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class InvenItemInfo
{
    public int itemId;
    public int itemIndex;
    public bool isEquip;

    public bool isRune;

    public InvenItemInfo(int id, int idx, bool isRune)
    {
        itemId = id;
        itemIndex = idx;
        this.isRune = isRune;
    }

    public SkillRecord GetSkillRecord()
    {
        if (isRune) return null;
        var skill = TableManager.Instance.skillTable.GetRecord(itemIndex);

        return skill;
    }

    public RuneRecord GetRuneRecord()
    {
        if (isRune == false) return null;
        var rune = TableManager.Instance.runeTable.GetRecord(itemIndex);

        return rune;
    }

    public ItemRecord getItemRecord => TableManager.Instance.itemTable.GetRecord(itemIndex);
}

public class UIInGameInventory : UIBase, LoopScrollPrefabSource, LoopScrollDataSource
{
    public LoopScrollRect loopScrollRect;

    private Dictionary<int, EquipSkill> equipSkillDic = new Dictionary<int, EquipSkill>();
    private Dictionary<int, InvenItemInfo> invenItemInfoDic = new Dictionary<int, InvenItemInfo>();
    private List<InvenItemInfo> invenItemList = new List<InvenItemInfo>();
    private Dictionary<Transform, UIInvenItemSlot> dicPrefab = new Dictionary<Transform, UIInvenItemSlot>();

    private Stack<Transform> pool = new Stack<Transform>();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 1; i <= Define.MaxEquipSkill; i++)
        {
            equipSkillDic.Add(i, new EquipSkill());
        }
    }

    public override void Open()
    {
        base.Open();
        ResetData();
    }

    public override void ResetData()
    {
        loopScrollRect.prefabSource = this;
        loopScrollRect.dataSource = this;
        loopScrollRect.totalCount = invenItemList.Count;
        loopScrollRect.RefillCells();
    }

    public void AddSkillItem(int itemId, int skillIdx)
    {
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, skillIdx, false);
        invenItemInfoDic.Add(itemId, itemInfo);
        invenItemList.Add(itemInfo);
    }

    public void AddRuneItem(int itemId, int runeIdx)
    {
        InvenItemInfo itemInfo = new InvenItemInfo(itemId, runeIdx, true);
        invenItemInfoDic.Add(itemId, itemInfo);
        invenItemList.Add(itemInfo);
    }

    public void OpenEquipRunePage(UIInvenItemSlot invenItemSlot)
    {

    }

    public void OpenEquipSkillPage(UIInvenItemSlot invenItemSlot)
    {

    }

    public void EquipSkill(InvenItemInfo itemInfo, int slotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;

        equipSkill.SetSkill(itemInfo.GetSkillRecord());
    }

    public void EquipRune(InvenItemInfo itemInfo, int slotIdx, int runeSlotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;
        if (equipSkill.CanEquipRune(itemInfo.GetRuneRecord()) == false) return;

        equipSkill.SetRune(itemInfo.GetRuneRecord(), runeSlotIdx);
    }

    public void UnEquipSkill(int slotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;
        if (equipSkill.skillRecord == null) return;

        equipSkill.skillRecord = null;
    }

    public void UnEquipRune(int slotIdx, int runeSlotIdx)
    {
        if (equipSkillDic.TryGetValue(slotIdx, out var equipSkill) == false) return;
        if (equipSkill.skillRecord == null) return;

        equipSkill.runeDic[runeSlotIdx] = null;
    }

    public GameObject GetObject(int index)
    {
        if (pool.Count == 0)
        {
            GameObject obj = uiManager.OpenUI("Assets/Data/GameResources/Prefab/Slot/UIInvenItemSlot.prefab").gameObject;
            dicPrefab.Add(obj.transform, obj.GetComponent<UIInvenItemSlot>());
            return obj;
        }

        Transform candidate = pool.Pop();
        candidate.gameObject.SetActive(true);
        return candidate.gameObject;
    }

    public void ReturnObject(Transform trans)
    {
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);

        pool.Push(trans);
    }

    public void ProvideData(Transform transform, int idx)
    {
        var invenItem = invenItemList[idx];
        UIInvenItemSlot slot = dicPrefab[transform];
        slot.Open(invenItem, this);
        slot.name = idx.ToString();
    }

}
