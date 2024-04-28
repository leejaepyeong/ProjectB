using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class InvenItemInfo
{
    public int itemId;
    public bool isEquip;

    public bool isRune;

    private int skillIndex;
    private int runeIndex;
    public InvenItemInfo(int id, int idx, bool isRune)
    {
        itemId = id;
        if (isRune)
            runeIndex = idx;
        else
            skillIndex = idx;
        this.isRune = isRune;
    }

    public SkillRecord GetSkillRecord()
    {
        if (isRune) return null;
        var skill = TableManager.Instance.skillTable.GetRecord(skillIndex);

        return skill;
    }

    public RuneRecord GetRuneRecord()
    {
        if (isRune == false) return null;
        var rune = TableManager.Instance.runeTable.GetRecord(runeIndex);

        return rune;
    }

    public string GetName()
    {
        return isRune ? GetRuneRecord().getName : GetSkillRecord().getName;
    }
    public string GetDest()
    {
        return isRune ? GetRuneRecord().getDest : GetSkillRecord().getDest;
    }
    public string GetIcon()
    {
        return isRune ? GetRuneRecord().iconPath : GetSkillRecord().iconPath;
    }
}

public class UIInGameInventory : UIDlg, LoopScrollPrefabSource, LoopScrollDataSource
{
    [SerializeField, FoldoutGroup("Inventory")] private UIInfinite.UIInfiniteScroll infiniteScroll;
    [SerializeField, FoldoutGroup("Inventory")] private ScrollRect scroll;

    private List<InvenItemInfo> invenItemList = new List<InvenItemInfo>();
    private Dictionary<Transform, UIInvenItemSlot> dicPrefab = new Dictionary<Transform, UIInvenItemSlot>();

    private Stack<Transform> pool = new Stack<Transform>();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Open()
    {
        base.Open();
        ResetData();
    }

    public override void ResetData()
    {
        invenItemList = BattleManager.Instance.playerData.invenItemList;

        SetInventoryUI();
    }

    private void SetInventoryUI()
    {
        infiniteScroll.Set(invenItemList.Count);
    }

    public void OpenEquipRunePage(UIInvenItemSlot invenItemSlot)
    {

    }

    public void OpenEquipSkillPage(UIInvenItemSlot invenItemSlot)
    {

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
