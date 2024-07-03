using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;
using System.Security.Cryptography;

namespace Editor
{
    [Serializable]
    public class StageData
    {
        [LabelText("�������� �ε���")] public int Seed;
        public string Name;
        [LabelText("�������� Ÿ��")] public eStageType Type;
        [LabelText("�÷��� �ð�"), MinValue(0)] public float playTime;

        [TableList] public List<SpawnInfo> spawnInfoList = new List<SpawnInfo>();

        [NonSerialized, FoldoutGroup("���� �̸�����"), LabelText("���� �ε���"),ShowInInspector] public int unitSeed;
        [NonSerialized, FoldoutGroup("���� �̸�����"), LabelText("���� ����"), ShowInInspector] public int unitLevel;
        [NonSerialized] private List<UnitData> unitDataList;
        [NonSerialized, FoldoutGroup("���� �̸�����"), LabelText("���� ����"), ReadOnly, ShowInInspector, ShowIf("@IsNullInfo() == false")] public UnitData unitData;

        public StageData(int seed, string name)
        {
            Seed = seed;
            Name = name;
        }

        [Serializable]
        public class SpawnInfo
        {
            public int monsterSeed;
            public int monsterLevel;
            [MinValue(0)] public int spawnMaxCount;
            [MinValue(0)] public float startDelay;
            [MinValue(0)] public float coolTime;
            public bool isBuff;

            [ShowIf("@isBuff")] public eBuffType startBuffType;
            [ShowIf("@isBuff")] public eStat startBuff;
            [ShowIf("@isBuff")] public eSkillDuration durationType;

        }

        [FoldoutGroup("���� �̸�����"), Button("���� ������ �ҷ�����"), InfoBox("�����Ͱ� �����ϴ�. ������ �ҷ�������.", InfoMessageType.Error, "IsNullData")]
        public void GetData()
        {
            unitDataList = new List<UnitData>();

            var assetLocationHandle = Addressables.LoadResourceLocationsAsync("JsonData_Unit", typeof(TextAsset));
            var assetLocations = assetLocationHandle.WaitForCompletion();

            var textAssetHandle = Addressables.LoadAssetsAsync<TextAsset>(assetLocations, null);
            var textAssets = textAssetHandle.WaitForCompletion();

            for (int i = 0; i < textAssets.Count; i++)
            {
                var jsonFile = textAssets[i];
                string fileName = string.Copy(jsonFile.name);
                string jsonText = Decrypt(string.Copy(jsonFile.text));

                var saveData = JsonUtility.FromJson<Editor.SaveUnitData>(jsonText);

                unitDataList.AddRange(saveData.dataList);
            }
        }

        [FoldoutGroup("���� �̸�����"), Button("���� ���� Ȯ��"), InfoBox("������ �����ϴ�. �ε����� Ȯ�����ּ���.", InfoMessageType.Error, "IsNullInfo")]
        public void ShowMonsterStat()
        {
            if (unitDataList == null)
                GetData();

            unitData = unitDataList.Find(_ => _.Seed == unitSeed);
        }
        private bool IsNullInfo()
        {
            if (unitDataList == null)
                return false;

            var unit = unitDataList.Find(_ => _.Seed == unitSeed);
            return unit == null;
        }
        private bool IsNullData()
        {
            return unitDataList == null;
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
    }



}