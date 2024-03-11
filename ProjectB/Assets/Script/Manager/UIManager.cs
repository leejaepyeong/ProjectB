using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum eWidgetType
{
    Back,
    Normal,
    Pop,
    Front,
}
public class UIManager : BaseManager
{
    [SerializeField] private UICanvas uiCanvasOrigin;
    [SerializeField] private Transform attach;
    [SerializeField] private Camera uiCamera;

    public Camera UiCamera => uiCamera;
    private List<UIBase> uiBases;
    private Queue<UICanvas> queueCanvas = new Queue<UICanvas>();
    private Dictionary<UIBase, UICanvas> dicCanvas = new Dictionary<UIBase, UICanvas>();
    private Dictionary<eWidgetType, int> dicWidgetCount = new Dictionary<eWidgetType, int>();

    public static UIManager Instance
    {
        get 
        { 
            return Manager.Instance.GetUIManger(); 
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

    public UIBase OpenUI(string path, eWidgetType widgetType = eWidgetType.Normal)
    {
        UICanvas uiCanvas = GetPooledCanvasContainer();

        UIBase uiBase = GameObjectPool.Get(path).GetComponent<UIBase>();
        uiBase.transform.SetParent(uiCanvas.transform);
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
        int sortOrder = GetSortOrder(widgetType);
        uiCanvas.canvas.sortingOrder = sortOrder;
        uiCanvas.widgetType = widgetType;
        dicCanvas.Add(uiBase, uiCanvas);
        AddUI(uiBase);
        return uiBase;
    }

    public T OpenWidget<T>(eWidgetType widgetType = eWidgetType.Normal) where T : UIBase
    {
        UICanvas uiCanvas = GetPooledCanvasContainer();

        T uiBase = GameObjectPool.Get(GetUIPath<T>()).GetComponent<T>();
        uiBase.transform.SetParent(uiCanvas.transform);
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
        int sortOrder = GetSortOrder(widgetType);
        uiCanvas.canvas.sortingOrder = sortOrder;
        uiCanvas.widgetType = widgetType;
        dicCanvas.Add(uiBase, uiCanvas);

        AddUI(uiBase);
        return uiBase;
    }
    private int GetSortOrder(eWidgetType widgetType)
    {
        if (dicWidgetCount.ContainsKey(widgetType) == false)
            dicWidgetCount.Add(widgetType, 1);
        else
            dicWidgetCount[widgetType] += 1;

        int widgetCount = dicWidgetCount[widgetType];
        int addValue = 0;
        switch (widgetType)
        {
            case eWidgetType.Normal:
                addValue = 1000;
                break;
            case eWidgetType.Pop:
                addValue = 2000;
                break;
            case eWidgetType.Front:
                addValue = 3000;
                break;
            case eWidgetType.Back:
            default:
                break;
        }
        return widgetCount + addValue;
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

    public void RemoveUIEscape()
    {
        if (uiBases.Count == 0) return;

        var uiBase = uiBases[uiBases.Count - 1];
        uiBase.OnClickEscape();
    }

    public void RemoveUI(UIBase uiBase)
    {
        GameObjectPool.Return(uiBase.gameObject);
        ReturnPooledCanvasContainer(dicCanvas[uiBase]);
        dicWidgetCount[dicCanvas[uiBase].widgetType] -= 1;
        dicCanvas.Remove(uiBase);
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
            RemoveUIEscape();
    }

    public void ResetDataUI()
    {
        for (int i = 0; i < uiBases.Count; i++)
        {
            uiBases[i].ResetData();
        }
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

    #region Canvas
    private UICanvas CreateCanvasContainer()
    {
        var objContainer = GameObject.Instantiate(uiCanvasOrigin.gameObject,
            Vector3.zero,
            Quaternion.identity,
            attach);
        return objContainer.GetComponent<UICanvas>();
    }

    private UICanvas GetPooledCanvasContainer()
    {
        if (queueCanvas.Count > 0)
        {
            return queueCanvas.Dequeue();
        }
        else
        {
            return CreateCanvasContainer();
        }
    }

    private void ReturnPooledCanvasContainer(UICanvas container)
    {
        container.component = null;
        container.SetActive(false);

        queueCanvas.Enqueue(container);
    }
    #endregion
}
