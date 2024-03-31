using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIHpBarDlg : UIBase
{
    private Queue<UIHpBar> hpBarQue = new Queue<UIHpBar>();
    private List<UIHpBar> hpBarList = new List<UIHpBar>();

    [SerializeField] private GameObject tempHpBar;

    public override void Open()
    {
        base.Open();
        hpBarQue.Clear();
        hpBarList.Clear();
    }

    public override void Close()
    {
        base.Close();
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
        UIHpBar hpBar = null;
        if (hpBarQue.Count > 0)
        {
            hpBarQue.Dequeue();
        }
        else
        {
            var tempObj = Instantiate(tempHpBar);
            hpBar = tempObj.GetComponent<UIHpBar>();
        }
        hpBar.Open(unit, this);
    }

    public void RecallHpBar(UIHpBar hpBar)
    {
        hpBarList.Remove(hpBar);
        hpBarQue.Enqueue(hpBar);
    }
}
