using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{
    void Notify(Subject subject);
}

public class Subject
{
    protected bool isNotify = false;
    protected List<Observer> observerList = new List<Observer>();

    public virtual void Attach(Observer observer)
    {
        if (observer == null) return;
        if (observerList.Contains(observer)) return;

        observerList.Add(observer);
    }

    public virtual void Clear()
    {
        observerList.Clear();
    }

    public virtual void Detach(Observer observer)
    {
        if (observer == null) return;
        observerList.Remove(observer);
    }

    public virtual void SetNotify(bool isNotify = true)
    {
        this.isNotify = isNotify;
    }

    public virtual void Notify()
    {
        List<Observer> temp = new List<Observer>(observerList);
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].ToString() == "null")
                continue;
            temp[i].Notify(this);
        }
    }

    public virtual void UpdateFrame()
    {
        if(isNotify)
        {
            Notify();
            isNotify = false;
        }
    }
}
