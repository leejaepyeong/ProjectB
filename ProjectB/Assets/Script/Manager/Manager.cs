using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BaseManager : MonoBehaviour
{
    public GameObjectPool GameObjectPool;
    public ResourcePool ResourcePool;

    protected float DeltaTime;

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
    private List<string> managerKeys = new List<string>(); 
    private SkillManager sm;
    public SkillManager skillManager
    {
        get
        {
            if (sm == null)
                sm = new SkillManager();
            return sm;
        }
    }
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
    private TableManager tableManager;
    public TableManager getTableManager
    {
        get
        {
            if (null == tableManager)
            {
                tableManager = new TableManager();
            }

            return tableManager;
        }
    }

    private BaseScene curScene;
    public BaseScene CurScene => curScene;
    public TestPlayerData playerData 
    {
        get
        {
            return curScene.isTestScene ? curScene.playerData : null;
        }
    }
    private Camera mainCamera;
    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                mainCamera.transform.SetParent(transform);
            }
            return mainCamera;
        }
    }
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
        managerKeys.Clear();
        DontDestroyOnLoad(gameObject);
        getTableManager.Load();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        for (int i = 0; i < managerKeys.Count; i++)
        {
            managerDic[managerKeys[i]].UpdateFrame(deltaTime);
        }

        if (fileData != null)
            fileData.UpdateFrame();
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
            managerKeys.Add(name);
        }
        return manager as T;
    }

    public UIManager GetUIManger()
    {
        if (managerDic.TryGetValue("UIManager", out var manager) == false)
        {
            var obj = Resources.Load<UIManager>("UIManager");
            manager = Instantiate(obj, transform);
            manager.transform.SetParent(transform);
            manager.Init();
            managerDic.Add("UIManager", manager);
            managerKeys.Add("UIManager");
        }
        return manager as UIManager;
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
}
