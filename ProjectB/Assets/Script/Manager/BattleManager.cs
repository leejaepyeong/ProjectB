using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : BaseManager
{
    private float stageTime;
    private float elaspedTime;
    private bool isInit;
    private bool isGameEnd;

    public float getCurTime { get { return elaspedTime; } }

    public static BattleManager Instance
    {
        get { return Manager.Instance.GetManager<BattleManager>(); }
    }

    public void SetGame(float time)
    {
        stageTime = time;
        isGameEnd = false;
        isInit = true;
    }

    public override void UpdateFrame(float deltaTime)
    {
        base.UpdateFrame(deltaTime);
        if (isInit == false) return;
        if (isGameEnd) return;

        elaspedTime += DeltaTime;
        if (elaspedTime > stageTime) elaspedTime = stageTime;

    }

    public bool CheckEndGame()
    {
        if (elaspedTime >= stageTime) { isGameEnd = true; return true; }
        if (UnitManager.Instance.Player != null && UnitManager.Instance.Player.UnitState.isDead) { isGameEnd = true; return true; }

        return false;
    }
}
