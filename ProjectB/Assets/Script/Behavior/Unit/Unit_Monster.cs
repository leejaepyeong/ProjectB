using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Monster : Unit_Base
{
    private float totalMoveDistance;
    public override void Move()
    {
        var direction = Vector3.Normalize(UnitManager.Instance.Player.GetPos() - unitBehavior.GetPos());
        float moveDistance = unitData.moveSpd * deltaTime;

        unitBehavior.transform.position += direction * moveDistance;

        totalMoveDistance += moveDistance;
    }
}
