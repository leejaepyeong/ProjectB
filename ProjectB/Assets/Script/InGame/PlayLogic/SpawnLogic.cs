using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnUnit
{
    public float elaspedTime;
    public bool isDelayOn;
    public Data.StageData.SpawnInfo spawnInfo;

    public SpawnUnit(Data.StageData.SpawnInfo spawnInfo)
    {
        elaspedTime = 0;
        isDelayOn = false;
    }

    protected virtual void Spawn()
    {
        float angle = spawnInfo.randomAngle ? Random.Range(spawnInfo.minAngle, spawnInfo.maxAngle) : spawnInfo.angle;
        float radius = spawnInfo.randomRadius ? Random.Range(spawnInfo.minRadius, spawnInfo.maxRadius) : spawnInfo.radius;
        Vector2 spawnPos = new Vector2(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius);

        UnitManager.Instance.SpawnUnit(spawnInfo.monsterSeed, spawnPos);
    }
}
public class SpawnNormalUnit : SpawnUnit
{
    private Data.StageData.SpawnNormalInfo normalInfo => spawnInfo as Data.StageData.SpawnNormalInfo;
    public SpawnNormalUnit(Data.StageData.SpawnNormalInfo spawnInfo) : base(spawnInfo)
    {
        this.spawnInfo = spawnInfo;
    }
    public void UpdateFrame(float deltaTime)
    {
        elaspedTime += deltaTime;
        if (isDelayOn == false)
        {
            if (elaspedTime > spawnInfo.startDelay)
            {
                isDelayOn = true;
                elaspedTime = 0;
            }
            return;
        }

        if (elaspedTime < normalInfo.coolTime) return;

        elaspedTime = 0;
        Spawn();
    }
}

public class SpawnLogic : MonoBehaviour
{
    [FoldoutGroup("ShowRadius"), SerializeField] protected float radius;

    private Data.StageData.NormalStage normalStage;
    private List<SpawnNormalUnit> spawnUnitList = new List<SpawnNormalUnit>();

    public virtual void Init()
    {
        normalStage = BattleManager.Instance.stageData.stageInfo as Data.StageData.NormalStage;
        spawnUnitList.Clear();

        for (int i = 0; i < normalStage.spawnInfoList.Count; i++)
        {
            SpawnNormalUnit spawnUnit = new SpawnNormalUnit(normalStage.spawnInfoList[i]);
            spawnUnitList.Add(spawnUnit);
        }
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < spawnUnitList.Count; i++)
        {
            spawnUnitList[i].UpdateFrame(deltaTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, radius / 2);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireCube(transform.position, new Vector3(radius, radius, radius));
    }
#endif
}
