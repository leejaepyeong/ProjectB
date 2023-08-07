using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : BaseManager
{
    private int unitId;
    private Dictionary<int, UnitBehavior> unitDic;

    public const string UNITBEHAVIOR_ASSET_KEY = "Assets/GameResources/Prefab/UnitBehavior.prefab";

    public static UnitManager Instance
    {
        get { return Manager.Instance.GetManager<UnitManager>(); }
    }

    public override void Init()
    {
        base.Init();
        unitId = 0;
        unitDic = new Dictionary<int, UnitBehavior>();
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

    public void SpawnUnit(object modelKey, out UnitBehavior unit)
    {
        Manager.Instance.GetManager<UnitManager>().GameObjectPool.TryGet(UNITBEHAVIOR_ASSET_KEY, out var unitObject);
        unit = unitObject.GetComponent<UnitBehavior>();
        unit.transform.SetParent(transform);

        unit.Init(modelKey, GetUnitId());
        unitId += 1;
    }

    public void RemoveUnit(UnitBehavior unit)
    {
        unitDic.Remove(unit.ID);
        unit.Close();
        GameObjectPool.Return(unit.gameObject);
    }
}
