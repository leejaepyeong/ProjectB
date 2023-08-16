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
        public IReadOnlyDictionary<K, E> Dictonary => dicData;
        public SingleData()
        {
            dicData = new Dictionary<K, E>();
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
                }
                catch(Exception e)
                {
                    Debug.LogError(e + "ID : " + data.Key);
                }
            }
        }
        public void ReadData(byte[] bytes)
        {
            E[] dataSet = MessagePack.MessagePackSerializer.Deserialize<E[]>(bytes);

            foreach (var data in dataSet)
            {
                try
                {
                    dicData.Add(data.Key, data);
                }
                catch (Exception e)
                {
                    Debug.LogError(e + "ID : " + data.Key);
                }
            }
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
        #region Datas
        public SingleData<int, Dummy> Dummy { get; private set; }
        public SingleData<int, StringText> StringText { get; private set; }
        #endregion

        public void ReadData()
        {
            if (Application.isEditor)
                StartCoroutine(LoadAllJsonCo());
            else
                StartCoroutine(LoadAllMessagePackCo());
            LoadEncrypFile();
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
                string jsonText = string.Copy(jsonFile.text);

                LoadJsonText(fileName, jsonText);
                yield return new WaitForEndOfFrame();
            }
        }

        private void LoadJsonText(string fileName, string jsonText)
        {
            switch (fileName)
            {
                case "Dummy":
                    Dummy = new();
                    Dummy.ReadData(jsonText);
                    break;
                case "StringText":
                    StringText = new();
                    StringText.ReadData(jsonText);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region Read MessagePack
        private IEnumerator LoadAllMessagePackCo()
        {
            var assetLocationHandle = Addressables.LoadResourceLocationsAsync("MessagePackData", typeof(TextAsset));
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
                byte[] bytes = new byte[jsonFile.bytes.Length];
                Buffer.BlockCopy(jsonFile.bytes, 0, bytes, 0, jsonFile.bytes.Length);

                LoadMessagePackText(fileName, bytes);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }

        private void LoadMessagePackText(string fileName, byte[] bytes)
        {
            switch (fileName)
            {
                case "Dummy":
                    Dummy = new();
                    Dummy.ReadData(bytes);
                    break;
                case "StringText":
                    StringText = new();
                    StringText.ReadData(bytes);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region Read Encryp File
        private void LoadEncrypFile()
        {
            StartCoroutine(LoadAllEncrypFileCo());
        }

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
    }

}