using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;

[System.Serializable]
public class SaveData
{
    public List<UnitDataEditor_Data> dataList = new();
}

public class UnitInfo
{
    public string Name;
    public int Seed;

    public UnitInfo(string Name, int seed)
    {
        this.Name = Name;
        Seed = seed;
    }
}


public class UnitDataEditor : EditorWindow
{
    #region Data
    private string path;
    private List<UnitDataEditor_Data> dataList = new();
    #endregion

    private string iconPath;
    private Texture2D UnitIcon;
    private Rect iconRect;
    private float iconSize = 100;
    
    private string searchText;

    [MenuItem("Tools/UnitInfoEditor")]
    public static void Open()
    {
        GetWindow<UnitDataEditor>().titleContent = new GUIContent("Unit Info");
    }

    private void OnEnable()
    {
        path = "./Assets/GameResources/DataJsons/";
        path = Path.Combine(Application.dataPath + "/GameResources/DataJsons/", this.GetType().Name + ".json");
        GetSaveData();
    }

    private void OnGUI()
    {
        Draw();
    }

    private void GetSaveData()
    {
        LoadJson();
    }

    private void Draw()
    {
        GUILayout.BeginVertical();
        DrawTool();
        GUILayout.BeginHorizontal();
        DrawUnitList();
        DrawUnitInfo();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    Vector2 scrollbar;
    int unitSelect;

    #region Draw

    private void DrawTool()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(100)))
        {
            SaveToJson();
        }
        if (GUILayout.Button("Create Unit", EditorStyles.toolbarButton, GUILayout.Width(100)))
        {
            CreateUnitAdd();
        }
        if (GUILayout.Button("Delete Unit", EditorStyles.toolbarButton, GUILayout.Width(100)))
        {
            DeleteUnit();
        }
        GUILayout.EndHorizontal();
    }

    private void DrawUnitList()
    {
        scrollbar = GUILayout.BeginScrollView(scrollbar, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.Width(400));

        GUILayout.BeginVertical();

        searchText = GUILayout.TextField(searchText, GUILayout.Width(100));
        for (int i = 0; i < dataList.Count; i++)
        {
            if (string.IsNullOrEmpty(searchText) == false && dataList[i].Name.Contains(searchText) == false)
                continue;

            UnitInfo info = new(dataList[i].Name, dataList[i].Seed);
            if(GUILayout.Toggle(i == unitSelect, dataList[i].Name, GUILayout.ExpandWidth(true), GUILayout.Width(100)))
            {
                PlayerPrefs.SetInt("DebugTapIdx", unitSelect = i);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private void DrawUnitInfo()
    {
        EditorGUILayout.BeginVertical();
        EditorGUIUtility.labelWidth = 30;
        dataList[unitSelect].Name = EditorGUILayout.TextField("Name", dataList[unitSelect].Name, GUILayout.Width(120));
        dataList[unitSelect].Seed = EditorGUILayout.IntField("Seed", dataList[unitSelect].Seed, GUILayout.Width(120));
        var unitInfo = dataList[unitSelect].info;
        unitInfo.hp  = EditorGUILayout.IntField("Hp", unitInfo.hp, GUILayout.Width(120));
        unitInfo.texture = (Texture2D)EditorGUILayout.ObjectField("Icon", unitInfo.texture, typeof(Texture2D), GUILayout.Width(120));
        unitInfo.objectValue = EditorGUILayout.ObjectField("Prefab", unitInfo.objectValue, typeof(Object), GUILayout.Width(120));
        unitInfo.pos = EditorGUILayout.Vector3Field("Pos", unitInfo.pos, GUILayout.Width(150));

        EditorGUILayout.EndVertical();
    }

    #endregion

    private void CreateUnitAdd()
    {
        UnitDataEditor_Data data = new UnitDataEditor_Data(dataList.Count, dataList.Count.ToString());
        dataList.Add(data);
    }

    private void DeleteUnit()
    {
        dataList.RemoveAt(unitSelect);
        unitSelect = 0;
    }


    #region Json
    private void SaveToJson()
    {
        SaveData saveData = new();

        for (int i = 0; i < dataList.Count; i++)
        {
            saveData.dataList = dataList;
        }

        string jsonText = JsonUtility.ToJson(saveData, true);
        string encryptString = Encrypt(jsonText);
        File.WriteAllText(path, encryptString);
    }

    private void LoadJson()
    {
        SaveData saveData = new();
        if (File.Exists(path))
        {
            string decryptString = File.ReadAllText(path);
            string jsonText = Decrypt(decryptString);
            saveData = JsonUtility.FromJson<SaveData>(jsonText);

            for (int i = 0; i < saveData.dataList.Count; i++)
            {
                dataList.Add(saveData.dataList[i]);
            }
        }
        else
        {
            UnitDataEditor_Data data = new UnitDataEditor_Data(0,"0");
            dataList.Add(data);
            SaveToJson();
        }
    }
    #endregion

    #region Encryption
    private static readonly string privateKey = "thisismyprivatekey";
    private static string Encrypt(string data)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateEncryptor();
        byte[] result = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(result, 0, result.Length);
    }

    private static string Decrypt(string data)
    {
        byte[] bytes = System.Convert.FromBase64String(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateDecryptor();
        byte[] result = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Text.Encoding.UTF8.GetString(result);
    }    

    private static RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(privateKey);
        RijndaelManaged result = new();

        byte[] newKeysArray = new byte[16];
        System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

        result.Key = newKeysArray;
        result.Mode = CipherMode.ECB;
        result.Padding = PaddingMode.PKCS7;
        return result;
    }

    #endregion
}
