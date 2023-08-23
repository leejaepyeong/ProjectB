using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public GameObjectPool GameObjectPool;
    public ResourcePool ResourcePool;

    protected float DeltaTime;

    #region MANAGER_ASSETKEY
    protected const string UI_MANAGER_ASSET_KEY = "Assets/GameResources/Prefab/Manager/UIManager.prefab";
    #endregion

    public virtual void Init()
    {
        ResourcePool = new();
        GameObjectPool = new(name);
    }

    public virtual void UnInit()
    {
        ResourcePool = null;
        GameObjectPool = null;
    }

    public virtual void UpdateFrame(float deltaTime)
    {
        DeltaTime = deltaTime;
    }
}

public class Manager : Singleton<Manager>
{
    #region Manager
    private Dictionary<string, BaseManager> managerDic = new Dictionary<string, BaseManager>();
    #endregion

    private FileData fileData;
    public FileData getFileData
    {
        get
        {
            if(fileData == null)
            {
                fileData = CreateComponent<FileData>(transform);
                fileData.Init();
            }
            return fileData;
        }
    }
    private Data.DataManager dataManager;
    public Data.DataManager getDataManager
    {
        get
        {
            if(dataManager == null)
            {
                dataManager = CreateComponent<Data.DataManager>(transform);
                dataManager.ReadData();
            }
            return dataManager;
        }
    }

    private BaseScene curScene;
    public void SetUI(BaseScene scene)
    {
        curScene = scene;
    }

    protected virtual void Awake()
    {
        Init();
    }

    private void Init()
    {
        managerDic.Clear();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var manager in managerDic)
        {
            manager.Value.UpdateFrame(deltaTime);
        }

        if (curScene != null)
            curScene.UpdateFrame(deltaTime);
    }

    public T GetManager<T>() where T : BaseManager
    {
        string name = typeof(T).Name;

        if (managerDic.TryGetValue(name, out var manager) == false)
        {
            manager = CreateComponent<T>(transform);
            manager.Init();
            managerDic.Add(name, manager);
        }
        return manager as T;
    }

    private T CreateComponent<T>(Transform _parent) where T : MonoBehaviour
    {
        GameObject _obj = new GameObject(typeof(T).Name);
        if (null != _parent)
            _obj.transform.SetParent(_parent);
        return _obj.AddComponent<T>();
    }

    public IEnumerator MoveScene(string sceneName)
    {
        AsyncOperation asynLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LobbyScene");
        asynLoad.allowSceneActivation = false;

        while(true)
        {

            yield return new WaitForEndOfFrame();


        }
    }

    public string GetString(int seed)
    {
        if (!Data.DataManager.Instance.StringText.TryGet(seed, out var str)) return "";

        return str.Kor;
    }
}
