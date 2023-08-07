using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBase : MonoBehaviour
{
    public virtual bool isOpen
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Init()
    {

    }

    public virtual void UpdateFrame(float deltaTime)
    {

    }
}
