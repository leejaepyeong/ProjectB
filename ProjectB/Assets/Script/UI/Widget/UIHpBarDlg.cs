using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIHpBarDlg : UIDlg
{
    private Queue<UIHpBar> hpBarQue = new Queue<UIHpBar>();
    private List<UIHpBar> hpBarList = new List<UIHpBar>();

    [SerializeField] private GameObject tempHpBar;
    public Transform attach;

    public override void Open()
    {
        base.Open();
        hpBarQue.Clear();
        hpBarList.Clear();
        tempHpBar.gameObject.SetActive(false);
    }

    public override void Close()
    {
        base.Close();
        while(hpBarQue.Count > 0)
        {
            var hpBar = hpBarQue.Dequeue();
            hpBar.Close();
        }
        for (int i = 0; i < hpBarList.Count; i++)
        {
            hpBarList[i].Close();
        }

        hpBarQue.Clear();
        hpBarList.Clear();
    }

    public override void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < hpBarList.Count; i++)
        {
            hpBarList[i].UpdateFrame(deltaTime);
        }
    }

    public void RequestHpBar(UnitBehavior unit)
    {
        if (unit == null) return;

        UIHpBar hpBar = null;
        if (hpBarQue.Count > 0)
        {
            hpBar = hpBarQue.Dequeue();
        }
        else
        {
            var tempObj = Instantiate(tempHpBar);
            hpBar = tempObj.GetComponent<UIHpBar>();
        }
        hpBar.Open(unit, this);
        hpBarList.Add(hpBar);
    }

    public void RecallHpBar(UIHpBar hpBar)
    {
        hpBarList.Remove(hpBar);
        hpBarQue.Enqueue(hpBar);
    }
}
