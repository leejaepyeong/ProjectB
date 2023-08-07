using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter
{

}

public class UIManager : BaseManager
{
    public Canvas canvas;

    private List<UIBase> uiBases;

    public static UIManager Instance
    {
        get { return Manager.Instance.GetManager<UIManager>(); }
    }

    public override void Init()
    {
        uiBases = new();
    }

    public void Clear()
    {
        RemoveUIAll();
        uiBases.Clear();
        uiBases = null;
        GameObjectPool = null;
        ResourcePool = null;
    }

    public void AddUI(UIBase uibase)
    {
        uiBases.Add(uibase);
    }

    public UIBase OpenUI(string path, Parameter param = default)
    {
        UIBase uiBase = GameObjectPool.Get(path).GetComponent<UIBase>();
        uiBase.transform.SetParent(canvas.transform);
        uiBase.RectTransform.anchoredPosition3D = Vector3.zero;
        uiBase.RectTransform.offsetMin = Vector2.zero;
        uiBase.RectTransform.offsetMax = Vector2.zero;
        uiBase.RectTransform.localRotation = Quaternion.identity;
        uiBase.RectTransform.localScale = Vector3.one;

        uiBase.SetParam(param);
        uiBase.Init();
        uiBase.Open();

        if (uiBase == null)
        {
            Debug.LogError("null == UIPath");
            return null;
        }            
        AddUI(uiBase);
        return uiBase;
    }

    public T OpenUI<T>(Parameter param = default) where T : UIBase
    {
        T uiBase = GameObjectPool.Get(GetUIPath<T>()).GetComponent<T>();
        uiBase.transform.SetParent(canvas.transform);
        uiBase.RectTransform.anchoredPosition3D = Vector3.zero;
        uiBase.RectTransform.offsetMin = Vector2.zero;
        uiBase.RectTransform.offsetMax = Vector2.zero;
        uiBase.RectTransform.localRotation = Quaternion.identity;
        uiBase.RectTransform.localScale = Vector3.one;

        uiBase.SetParam(param);
        uiBase.uiManager = this;
        uiBase.Init();
        uiBase.Open();

        if (uiBase == null)
        {
            Debug.LogError("null == UIPath");
            return null;
        }
        AddUI(uiBase);
        return uiBase;
    }

    public T GetUI<T>() where T : UIBase 
    {
        for (int i = 0; i < uiBases.Count; i++)
        {
            if (uiBases is T typeUI)
                return typeUI;
        }

        return null;
    }

    public void RemoveUI()
    {
        if (uiBases.Count == 0) return;

        uiBases[uiBases.Count - 1].Close();
        GameObjectPool.Return(uiBases[uiBases.Count - 1].gameObject);
        uiBases.Remove(uiBases[uiBases.Count - 1]);
    }

    public void RemoveUI(UIBase uiBase)
    {
        GameObjectPool.Return(uiBase.gameObject);
        uiBases.Remove(uiBase);
    }

    public void RemoveUIAll()
    {
        while(uiBases.Count > 0)
        {
            RemoveUI(uiBases[0]);
        }
    }

    public override void UpdateFrame(float deltaTime)
    {
        for (int i = 0; i < uiBases.Count; i++)
        {
            uiBases[i].UpdateFrame(deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            RemoveUI();
    }

    private string GetUIPath(UIBase uIBase)
    {
        return string.Format("Assets/GameResouces/Prefab/UI/" + uIBase.name + ".prefab");
    }

    private string GetUIPath<T>()
    {
        System.Type _key = typeof(T);
        return string.Format("Assets/GameResources/Prefab/UI/" + _key.Name + ".prefab");
    }
}
