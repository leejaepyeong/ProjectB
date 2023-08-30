using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TestPlayScene : BaseScene
{
    [SerializeField, FoldoutGroup("User")] private int user_Seed;
    [SerializeField, FoldoutGroup("User")] private Transform trf_user;
    [SerializeField, FoldoutGroup("User")] public TestPlayerData playerData;

    [SerializeField, FoldoutGroup("Enemy")] private int enemy_Seed;
    [SerializeField, FoldoutGroup("Enemy")] private Transform[] trf_spawns;
    [SerializeField, FoldoutGroup("Enemy/Setting")] private long hp;
    [SerializeField, FoldoutGroup("Enemy/Setting")] private long atk;
    [SerializeField, FoldoutGroup("Enemy/Setting")] private long def;
    [SerializeField, FoldoutGroup("Enemy/Setting")] private float atkSpd;
    [SerializeField, FoldoutGroup("Enemy/Setting")] private float moveSpd;

    [SerializeField, FoldoutGroup("Setting")] private bool isTest;
    [SerializeField, FoldoutGroup("Setting")] private float timer;

    private float deltaTime;

    private UnitBehavior enemyUnit;

    [Button]
    public void SpawnUser()
    {
        UnitManager.Instance.SpawnUnit(user_Seed);
    }

    [Button]
    public void SpawnMonster()
    {
        enemyUnit = UnitManager.Instance.SpawnUnit(enemy_Seed);
        enemyUnit.transform.position = trf_spawns[Random.Range(0,trf_spawns.Length)].position;
    }

    [Button]
    public void Reset()
    {

    }

    #region Commnad
    QueueCommand commands = new QueueCommand();
    IntroCommand_LoadDataFile loadDataFile = new IntroCommand_LoadDataFile();
    IntroCommand_LoadLocalData loadLocalData = new IntroCommand_LoadLocalData();
    #endregion
    public override void Init()
    {
        commands.Add(loadDataFile);
        commands.Add(loadLocalData);
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isTest == false) return;

        this.deltaTime = deltaTime;
    }
}
