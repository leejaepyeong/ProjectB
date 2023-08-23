using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TestPlayScene : BaseScene
{
    [SerializeField, FoldoutGroup("User")] private int seed;
    [SerializeField, FoldoutGroup("User")] private Transform trf_user;

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
    public void SpawnMonster()
    {
        UnitManager.Instance.SpawnUnit(seed, out enemyUnit);
        enemyUnit.transform.position = trf_spawns[Random.Range(0,trf_spawns.Length)].position;
    }

    [Button]
    public void Reset()
    {

    }

    public override void Init()
    {
        SpawnUser();
    }

    public override void UpdateFrame(float deltaTime)
    {
        if (isTest == false) return;

        this.deltaTime = deltaTime;
    }

    private void SpawnUser()
    {

    }
}
