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
    public eItemType itemType;
    public int itemIndex;
    public InvenItemInfo(int id, int idx, eItemType itemType)
    {
        itemId = id;

        itemIndex = idx;
        this.itemType = itemType;
    }

    public SkillRecord GetSkillRecord()
    {
        if (itemType != eItemType.Skill) return null;
        var skill = TableManager.Instance.skillTable.GetRecord(itemIndex);

        return skill;
    }

    public RuneRecord GetRuneRecord()
    {
        if (itemType != eItemType.Rune) return null;
        var rune = TableManager.Instance.runeTable.GetRecord(itemIndex);

        return rune;
    }
    public StatRewardRecord GetStatRecord()
    {
        if (itemType != eItemType.Use) return null;
        var rune = TableManager.Instance.statRewardTable.GetRecord(itemIndex);

        return rune;
    }
    public string GetName()
    {
        switch (itemType)
        {
            case eItemType.Rune: return GetRuneRecord().getName;
            case eItemType.Skill: return GetSkillRecord().getName;
            case eItemType.Use: return GetStatRecord().getName;
            case eItemType.None:
            default:
                return "";
        }
    }
    public string GetDest()
    {
        switch (itemType)
        {
            case eItemType.Rune: return GetRuneRecord().getDest;
            case eItemType.Skill: return GetSkillRecord().getDest;
            case eItemType.Use: return GetStatRecord().getDest;
            case eItemType.None:
            default:
                return "";
        }
    }
    public string GetIcon()
    {
        switch (itemType)
        {
            case eItemType.Rune: return GetRuneRecord().iconPath;
            case eItemType.Skill: return GetSkillRecord().iconPath;
            case eItemType.Use: return GetStatRecord().iconPath;
            case eItemType.None:
            default:
                return "";
        }
    }
}

public class UIInGameInventory : UIDlg
{
    [SerializeField, FoldoutGroup("Inventory")] private UIInfinite.UIInfiniteScroll infiniteScroll;
    [SerializeField, FoldoutGroup("Inventory")] private ScrollRect scroll;

    private List<InvenItemInfo> invenItemList = new List<InvenItemInfo>();
    public List<InvenItemInfo> InvenItemList => invenItemList;
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
        infiniteScroll.Refresh();
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
}
