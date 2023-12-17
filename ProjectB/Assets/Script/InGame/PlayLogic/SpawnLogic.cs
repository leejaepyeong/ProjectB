using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnLogic : MonoBehaviour
{
    [FoldoutGroup("Setting"), SerializeField] protected float coolTime;
    [FoldoutGroup("Setting"), SerializeField] protected float maxRadius;
    [FoldoutGroup("Setting"), SerializeField] protected float minRadius;

    protected float elaspedTime;

    public virtual void UpdateFrame(float deltaTime)
    {
        elaspedTime += deltaTime;
        SpawnMonster();
    }

    protected void SpawnMonster()
    {
        if (elaspedTime < coolTime) return;
        elaspedTime = 0;

        float angle = Random.Range(0,360f);
        float radius = Random.Range(minRadius, maxRadius);
        Vector2 spawnPos = new Vector2(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius);

        UnitManager.Instance.SpawnUnit(101, spawnPos);
    }
}
