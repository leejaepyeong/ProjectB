using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UIManager : BaseManager
{
    private const string UICANVAS_PATH = "Assets/Data/GameResources/Prefab/Widget/UICanvas.prefab";
    private UICanvas uiCanvas;
    public UICanvas UiCanvas
    {
        get
        {
            if(uiCanvas == null)
            {
                if (GameObjectPool.TryGet(UICANVAS_PATH, out var obj))
                    uiCanvas = obj.GetComponent<UICanvas>();
                uiCanvas.transform.SetParent(transform);
            }
            return uiCanvas;
        }
    }

    public Canvas canvas
    {
        get
        {
            return UiCanvas.canvas;
        }
    }
    public Camera uiCamera
    {
        get
        {
            return UiCanvas.uiCamera;
        }
    }

    private List<UIBase> uiBases;

    public static UIManager Instance
    {
        get 
        { 
            return Manager.Instance.GetManager<UIManager>(); 
        }
    }

    public override void Init()
    {
        base.Init();
        uiBases = new();
        var cameraData = Manager.Instance.MainCamera.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(uiCamera);
    }

    public void DeInit()
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

    public UIBase OpenUI(string path)
    {
        UIBase uiBase = GameObjectPool.Get(path).GetComponent<UIBase>();
        uiBase.transform.SetParent(canvas.transform);
        uiBase.RectTransform.anchoredPosition3D = Vector3.zero;
        uiBase.RectTransform.offsetMin = Vector2.zero;
        uiBase.RectTransform.offsetMax = Vector2.zero;
        uiBase.RectTransform.localRotation = Quaternion.identity;
        uiBase.RectTransform.localScale = Vector3.one;

        if (uiBase == null)
        {
            Debug.LogError("null == UIPath");
            return null;
        }            
        AddUI(uiBase);
        return uiBase;
    }

    public T OpenWidget<T>() where T : UIBase
    {
        T uiBase = GameObjectPool.Get(GetUIPath<T>()).GetComponent<T>();
        uiBase.transform.SetParent(canvas.transform);
        uiBase.RectTransform.anchoredPosition3D = Vector3.zero;
        uiBase.RectTransform.offsetMin = Vector2.zero;
        uiBase.RectTransform.offsetMax = Vector2.zero;
        uiBase.RectTransform.localRotation = Quaternion.identity;
        uiBase.RectTransform.localScale = Vector3.one;

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
        return string.Format("Assets/Data/GameResources/Prefab/Widget/" + _key.Name + ".prefab");
    }

    public UIMessageBox OpenMessageBox_Ok(string title = null, string dest = null, string okText = null, UnityEngine.Events.UnityAction okAction = null)
    {
        UIMessageBox msg = GetUI<UIMessageBox>();
        msg.Open(title, dest, okText, okAction, null, null);

        return msg;
    }

    public UIMessageBox OpenMessageBox_OkCancle(string title = null, string dest = null, string okText = null, UnityEngine.Events.UnityAction okAction = null,
        string cancleText = null, UnityEngine.Events.UnityAction cancleAction = null)
    {
        UIMessageBox msg = GetUI<UIMessageBox>();
        msg.Open(title, dest, okText, okAction, cancleText, cancleAction);

        return msg;
    }
}
