using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    [SerializeField]
    protected Transform scaleTransform;

    protected float elaspedTime;
    protected float deltaTime;

    protected GameObject Model;
    protected bool isInit;
    public bool IsInit => isInit;

    public virtual void UpdateFrame(float deltaTime)
    {
        this.deltaTime = deltaTime;
        elaspedTime += deltaTime;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }
}
