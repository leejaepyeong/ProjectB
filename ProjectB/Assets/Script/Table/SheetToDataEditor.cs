using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessagePack;
using MiniExcelLibs;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SheetToDataEditor : OdinEditorWindow
{
    [MenuItem("Tools/ProjectB/SheetToData")]
    public static void ShowEditor()
    {
        var wnd = GetWindow<SheetToDataEditor>();
        wnd.titleContent = new GUIContent("SheetToDataEditor");
    }

    [TitleGroup("Input Sheet File"), Sirenix.OdinInspector.FilePath, SerializeField] public string[] SheetAssetPathList;
    [TitleGroup("Output MessagePack Data"), FolderPath, SerializeField] public string BinaryOutputFolder;
    [TitleGroup("Output Json Data"), FolderPath, SerializeField] public string JsonOutputFolder;

    [TitleGroup("Export"), Button(ButtonSizes.Large)]
    public void SheetToData()
    {
        CacheTable();
        TableToData();
        UncacheTable();

        AssetDatabase.SaveAssets();
    }

    private Dictionary<string, IEnumerable<IDictionary<string, object>>> cachedTable = new();
    private void CacheTable()
    {
        cachedTable.Clear();
        foreach (var sheetAsset in SheetAssetPathList)
        {
            if (string.IsNullOrEmpty(sheetAsset)) return;
            if (string.IsNullOrWhiteSpace(sheetAsset)) return;

            var tableNames = MiniExcel.GetSheetNames(sheetAsset);
            foreach (var tableName in tableNames)
            {
                var table = MiniExcel.Query(sheetAsset, sheetName: tableName, useHeaderRow: true).ToList();
                cachedTable.Add(tableName, table);
            }
        }
    }

    private void TableToData()
    {
        foreach (var (tableName, table) in cachedTable)
        {
            switch (tableName)
            {
                case "Dummy":
                    CreateDataFile<Data.Dummy>(tableName, table, ConvertTableToDummy);
                    break;
                case "StringText":
                    CreateDataFile<Data.StringText>(tableName, table, ConvertTableToString);
                    break;
                default:
                    break;
            }
        }
    }

    private void UncacheTable()
    {
        cachedTable.Clear();
    }

    public abstract class TableConvert<E>
    {
        public abstract E[] Convert(IEnumerable<IDictionary<string, object>> table);
    }

    public void CreateDataFile<E>(string tableName,
           IEnumerable<IDictionary<string, object>> table,
           Func<IEnumerable<IDictionary<string, object>>, E[]> convertFunc)
    {
        var data = convertFunc(table);

        //messagepack bytes
        var dataBytes = MessagePackSerializer.Serialize(data);
        CreateFile(tableName, dataBytes);

        //messagepack json
        var dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        CreateFile(tableName, dataJson);
    }

    public void CreateFile(string tableName, byte[] bytes)
    {
        if (string.IsNullOrEmpty(BinaryOutputFolder)) return;

        string outputPath = BinaryOutputFolder;
        outputPath = "./" + outputPath;
        Directory.CreateDirectory(outputPath);
        string fileName = tableName + ".bytes";
        using BinaryWriter binaryWriter = new BinaryWriter(File.Open(outputPath + "/" + fileName, FileMode.OpenOrCreate));
        binaryWriter.Write(bytes);
        Debug.Log("Create Binary Data :" + tableName);
    }

    public void CreateFile(string tableName, string json)
    {
        if (string.IsNullOrEmpty(JsonOutputFolder)) return;

        string outputPath = JsonOutputFolder;
        outputPath = "./" + outputPath;
        Directory.CreateDirectory(outputPath);
        string fileName = tableName + ".bytes";
        using StreamWriter streamWriter = new StreamWriter(outputPath + "/" + fileName, false, System.Text.Encoding.UTF8);
        streamWriter.Write(json);
        Debug.Log("Create Json Data :" + tableName);
    }

    #region Dummy
    public Data.Dummy[] ConvertTableToDummy(IEnumerable<IDictionary<string, object>> table)
    {
        List<Data.Dummy> listData = new();

        foreach (var row in table)
        {
            int seed = 0;
            float value = 0;
            foreach (var data in row)
            {
                string header = data.Key;
                if (string.IsNullOrEmpty(header)) continue;
                if (string.IsNullOrWhiteSpace(header)) continue;
                var numberValue = data.Value is double ? Convert.ToDouble(data.Value) : 0;
                var boolValue = data.Value is bool && Convert.ToBoolean(data.Value);
                var stringValue = data.Value is string ? Convert.ToString(data.Value) : string.Empty;

                switch (header)
                {
                    case "Seed":
                        seed = (int)numberValue;
                        break;
                    case "Value":
                        value = (float)numberValue;
                        break;
                }
            }
            if (seed == 0) continue;
            listData.Add(new Data.Dummy(seed, value));
        }

        return listData.OrderBy(r => r.Seed).ToArray();
    }
    #endregion
    #region String
    public Data.StringText[] ConvertTableToString(IEnumerable<IDictionary<string, object>> table)
    {
        List<Data.StringText> listData = new();

        foreach (var row in table)
        {
            int seed = 0;
            string kor = "";
            string eng = "";
            foreach (var data in row)
            {
                string header = data.Key;
                if (string.IsNullOrEmpty(header)) continue;
                if (string.IsNullOrWhiteSpace(header)) continue;
                var numberValue = data.Value is double ? Convert.ToDouble(data.Value) : 0;
                var boolValue = data.Value is bool && Convert.ToBoolean(data.Value);
                var stringValue = data.Value is string ? Convert.ToString(data.Value) : string.Empty;

                switch (header)
                {
                    case "Seed":
                        seed = (int)numberValue;
                        break;
                    case "kor":
                        kor = stringValue;
                        break;
                    case "eng":
                        eng = stringValue;
                        break;
                }
            }
            if (seed == 0) continue;
            listData.Add(new Data.StringText(seed, kor, eng));
        }

        return listData.OrderBy(r => r.Seed).ToArray();
    }
    #endregion
}
