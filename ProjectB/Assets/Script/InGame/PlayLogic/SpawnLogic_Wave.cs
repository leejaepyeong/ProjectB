using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnWaveUnit : SpawnUnit
{
    private Data.StageData.SpawnWaveInfo waveInfo => spawnInfo as Data.StageData.SpawnWaveInfo;
    private bool isSpawn;
    private UnityAction<int, SpawnWaveUnit> removeAction;
    public SpawnWaveUnit(Data.StageData.SpawnWaveInfo spawnInfo, UnityAction<int, SpawnWaveUnit> removeAction) : base(spawnInfo)
    {
        this.spawnInfo = spawnInfo;
        isSpawn = false;
        this.removeAction = removeAction;
    }
    public void UpdateFrame(float deltaTime)
    {
        if (isSpawn) return;

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

        if (waveInfo.waveNumber < BattleManager.Instance.waveNumber) return;

        isSpawn = true;
        for (int i = 0; i < waveInfo.spawnCount; i++)
        {
            Spawn();
        }
        removeAction?.Invoke(waveInfo.waveNumber, this);
    }
}
public class SpawnLogic_Wave : SpawnLogic
{
    private Data.StageData.WaveStage waveStage;
    private Dictionary<int, List<SpawnWaveUnit>> spawnUnitDic = new Dictionary<int, List<SpawnWaveUnit>>();

    public override void Init()
    {
        waveStage = BattleManager.Instance.stageData.stageInfo as Data.StageData.WaveStage;
        spawnUnitDic.Clear();

        for (int i = 0; i < waveStage.spawnInfoList.Count; i++)
        {
            SpawnWaveUnit spawnUnit = new SpawnWaveUnit(waveStage.spawnInfoList[i], RemoveSpawnUnit);

            if (spawnUnitDic.TryGetValue(waveStage.spawnInfoList[i].waveNumber, out var list) == false)
            {
                list = new List<SpawnWaveUnit>();
                spawnUnitDic.Add(waveStage.spawnInfoList[i].waveNumber, list);
            }
            list.Add(spawnUnit);
        }
    }
    public override void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < spawnUnitDic[BattleManager.Instance.waveNumber].Count; i++)
        {
            spawnUnitDic[BattleManager.Instance.waveNumber][i].UpdateFrame(deltaTime);
        }
    }

    private void RemoveSpawnUnit(int waveNumber, SpawnWaveUnit spawnWaveUnit)
    {
        if (spawnUnitDic.TryGetValue(waveNumber, out var list) == false) return;
        list.Remove(spawnWaveUnit);
    }
}
