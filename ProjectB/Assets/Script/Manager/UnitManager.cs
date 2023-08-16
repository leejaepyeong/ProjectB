using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : BaseManager
{
    private int unitId;
    private Dictionary<int, UnitBehavior> unitDic;
    private List<UnitBehavior> unitActiveList;
    public List<UnitBehavior> UnitActiveList => unitActiveList;

    public const string UNITBEHAVIOR_ASSET_KEY = "Assets/Data/GameResources/Prefab/Behavior/UnitBehavior.prefab";

    public static UnitManager Instance
    {
        get { return Manager.Instance.GetManager<UnitManager>(); }
    }

    public override void Init()
    {
        base.Init();
        unitId = 1;
        unitDic = new Dictionary<int, UnitBehavior>();
    }

    public override void UnInit()
    {
        unitDic.Clear();
        unitDic = null;
        unitActiveList.Clear();
        unitActiveList = null;
        base.UnInit();
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        foreach (var unit in unitDic)
        {
            unit.Value.UpdateFrame(DeltaTime, 1);
        }
    }

    public int GetUnitId()
    {
        return unitId;
    }

    public void SpawnUnit(int seed, out UnitBehavior unit)
    {
        Manager.Instance.GetManager<UnitManager>().GameObjectPool.TryGet(UNITBEHAVIOR_ASSET_KEY, out var unitObject);
        unit = unitObject.GetComponent<UnitBehavior>();
        unit.transform.SetParent(transform);

        unit.Init(seed, GetUnitId());
        unitDic.Add(unitId, unit);
        UnitActive(unit, true);
        unitId += 1;
    }

    public void RemoveUnit(UnitBehavior unit)
    {
        if (unitActiveList.Contains(unit))
            UnitActive(unit,false);

        unitDic.Remove(unit.ID);
        unit.Close();
        GameObjectPool.Return(unit.gameObject);
    }

    public void UnitActive(UnitBehavior unit, bool isActive)
    {
        if (isActive)
            unitActiveList.Add(unit);
        else
            unitActiveList.Remove(unit);
    }
}
