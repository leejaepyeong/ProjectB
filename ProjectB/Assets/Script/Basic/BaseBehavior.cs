using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehavior : MonoBehaviour
{
    [SerializeField]
    protected Transform scaleTransform;
    [SerializeField]
    protected CircleCollider2D col;

    protected float elaspedTime;
    protected float deltaTime;

    public GameObject Model;
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

    public Quaternion GetRot()
    {
        return transform.rotation;
    }
}
