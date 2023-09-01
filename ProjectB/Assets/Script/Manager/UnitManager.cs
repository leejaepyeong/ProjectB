using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : BaseManager
{
    private int unitId;
    private Dictionary<int, UnitBehavior> unitDic;
    private UnitBehavior player;
    public UnitBehavior Player => player;
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
        unitActiveList = new List<UnitBehavior>();
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

    public UnitBehavior SpawnPlayer(int seed)
    {
        Manager.Instance.GetManager<UnitManager>().GameObjectPool.TryGet(UNITBEHAVIOR_ASSET_KEY, out var unitObject);
        UnitBehavior unit = unitObject.GetComponent<UnitBehavior>();
        unit.transform.SetParent(transform);

        Data.DataManager.Instance.UnitData.TryGet(seed, out var unitData);
        unit.Init(unitData, GetUnitId());
        unitDic.Add(unitId, unit);
        UnitActive(unit, true);
        unitId += 1;

        player = unit;
        return unit;
    }

    public UnitBehavior SpawnUnit(int seed, Vector2 spawnPos)
    {
        Manager.Instance.GetManager<UnitManager>().GameObjectPool.TryGet(UNITBEHAVIOR_ASSET_KEY, out var unitObject);
        UnitBehavior unit = unitObject.GetComponent<UnitBehavior>();
        unit.transform.SetParent(transform);
        if (spawnPos != default) unit.transform.localPosition = spawnPos;

        Data.DataManager.Instance.UnitData.TryGet(seed, out var unitData);
        unit.Init(unitData, GetUnitId());
        unitDic.Add(unitId, unit);
        UnitActive(unit, true);
        unitId += 1;

        return unit;
    }

    public void RemoveUnit(UnitBehavior unit)
    {
        if (unitActiveList.Contains(unit))
            UnitActive(unit,false);

        StartCoroutine(RemoveUnitCo(unit));
    }

    IEnumerator RemoveUnitCo(UnitBehavior unit)
    {
        yield return new WaitForEndOfFrame();

        unitDic.Remove(unit.ID);
        unit.Close();
        GameObjectPool.Return(unit.gameObject);

        yield return new WaitForEndOfFrame();
    }

    public void UnitActive(UnitBehavior unit, bool isActive)
    {
        if (isActive)
            unitActiveList.Add(unit);
        else
            unitActiveList.Remove(unit);
    }
}
