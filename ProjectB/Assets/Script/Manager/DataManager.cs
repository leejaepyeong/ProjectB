using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Security.Cryptography;

namespace Data
{
    public class SingleData<K, E> where E : IDataKey<K>
    {
        public readonly E DEFAULT = default(E);
        protected Dictionary<K, E> dicData;
        protected List<E> dataList;
        public IReadOnlyDictionary<K, E> Dictonary => dicData;
        public List<E> DataList => dataList;
        public SingleData()
        {
            dicData = new Dictionary<K, E>();
            dataList = new List<E>();
        }

        public void ReadData(string json)
        {
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Populate,
            };
            E[] dataSet = Newtonsoft.Json.JsonConvert.DeserializeObject<E[]>(json, settings);

            foreach (var data in dataSet)
            {
                try
                {
                    dicData.Add(data.Key, data);
                    dataList.Add(data);
                }
                catch(Exception e)
                {
                    Debug.LogError(e + "ID : " + data.Key);
                }
            }
        }

        public void ReadEncrptData(K key, E data)
        {
            dicData.Add(key, data);
        }

        public bool TryGet(in K key, out E data)
        {
            if (dicData.TryGetValue(key, out data))
                return true;
            else
            {
                Debug.LogError("No Data in dic");
                return false;
            }
        }
    }


    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance
        {
            get
            {
                if (Manager.Instance == null)
                    return null;
                return Manager.Instance.getDataManager;
            }
        }
        #region Datas
        public SingleData<int, UnitData> UnitData { get; private set; }
        public SingleData<int, StageData> StageData { get; private set; }

        #endregion

        private bool isEncryptFileDone;
        private bool isReadDataDone;
        public bool IsReadDone => isReadDataDone;

        private bool isDownLoadLeft;
        public bool IsDownLoadLeft => isDownLoadLeft;

        public void ReadData()
        {
            StartCoroutine(LoadAllJsonCo());
        }
        #region Read Json
        private IEnumerator LoadAllJsonCo()
        {
            var assetLocationHandle = Addressables.LoadResourceLocationsAsync("JsonData", typeof(TextAsset));
            while(assetLocationHandle.IsDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
            var assetLocations = assetLocationHandle.Result;

            var textAssetHandle = Addressables.LoadAssetsAsync<TextAsset>(assetLocations, null);
            while (textAssetHandle.IsDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
            var textAssets = textAssetHandle.Result;

            for (int i = 0; i < textAssets.Count; i++)
            {
                var jsonFile = textAssets[i];
                string fileName = string.Copy(jsonFile.name);
                string jsonText = Decrypt(string.Copy(jsonFile.text));

                LoadJsonText(fileName, jsonText);
                yield return new WaitForEndOfFrame();
            }

            isReadDataDone = true;
        }

        private void LoadJsonText(string fileName, string jsonText)
        {
            switch (fileName)
            {
                case "UnitDataEditor":
                    {
                        UnitData = new();

                        var saveData = JsonUtility.FromJson<Editor.SaveUnitData>(jsonText);

                        for (int i = 0; i < saveData.dataList.Count; i++)
                        {
                            UnitData data = new UnitData(saveData.dataList[i]);
                            UnitData.ReadEncrptData(saveData.dataList[i].Seed, data);
                        }
                    }
                    break;
                case "StageDataEditor":
                    {
                        StageData = new();

                        var saveData = JsonUtility.FromJson<Editor.SaveStageData>(jsonText);

                        for (int i = 0; i < saveData.dataList.Count; i++)
                        {
                            StageData data = new StageData(saveData.dataList[i]);
                            StageData.ReadEncrptData(saveData.dataList[i].Seed, data);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region Read Encryp File

        private IEnumerator LoadAllEncrypFileCo()
        {
            var assetLocationHandle = Addressables.LoadResourceLocationsAsync("Encryp", typeof(TextAsset));
            while (assetLocationHandle.IsDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
            var assetLocations = assetLocationHandle.Result;

            var textAssetHandle = Addressables.LoadAssetsAsync<TextAsset>(assetLocations, null);
            while (textAssetHandle.IsDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
            var textAssets = textAssetHandle.Result;

            for (int i = 0; i < textAssets.Count; i++)
            {
                var jsonFile = textAssets[i];
                string fileName = string.Copy(jsonFile.name);
                string jsonText = Decrypt(string.Copy(jsonFile.text));

                LoadJsonText(fileName, jsonText);
                yield return new WaitForEndOfFrame();
            }

            isEncryptFileDone = true;
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
            byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(Define.privateKey);
            RijndaelManaged result = new();

            byte[] newKeysArray = new byte[16];
            System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

            result.Key = newKeysArray;
            result.Mode = CipherMode.ECB;
            result.Padding = PaddingMode.PKCS7;
            return result;
        }
        #endregion
        #region Read All Data
        public void LoadAllData()
        {
            Addressables.GetDownloadSizeAsync("Remote").Completed += (opSize) =>
            {
                if (opSize.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded && opSize.Result > 0)
                {
                    Addressables.DownloadDependenciesAsync("Remote", true).Completed += (opDownload) =>
                    {
                        if (opDownload.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) return;
                        isDownLoadLeft = true;
                    };
                }
                else
                    isDownLoadLeft = true;

            };
        }
        #endregion
    }

}