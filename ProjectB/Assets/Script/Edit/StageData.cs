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
        [HorizontalGroup("Info", 180, 10), LabelText("�������� �ε���"), LabelWidth(100)] public int Seed;
        [HorizontalGroup("Info", 160, 5), LabelWidth(50)] public string Name;
        [HorizontalGroup("Info", 180, 5),LabelText("�������� Ÿ��"), LabelWidth(80)] public eStageType stageType;
        [HorizontalGroup("Info", 160, 5), LabelText("�÷��� �ð�"), MinValue(0), LabelWidth(80)] public float playTime;

        [SerializeField, ShowIf("@stageType == eStageType.Normal")] private NormalStage normalStage;
        [SerializeField, ShowIf("@stageType == eStageType.Wave")] private WaveStage waveStage;

        public StageType getStageType
        {
            get
            {
                switch (stageType)
                {
                    case eStageType.Normal:
                        return normalStage;
                    case eStageType.Wave:
                        return waveStage;
                    default:
                        return normalStage;
                }
            }
        }

        [HorizontalGroup("PreviewUnit", 500, 10)] public PreviewUnit previewUnit;



        public StageData(int seed, string name)
        {
            Seed = seed;
            Name = name;
        }

        #region Spawn
        [Serializable]
        public abstract class SpawnInfo
        {
            [LabelWidth(100), VerticalGroup("UnitData")] public int monsterSeed;
            [LabelWidth(100), VerticalGroup("UnitData")] public int monsterLevel;
            [MinValue(0)] public float startDelay;
            [VerticalGroup("Buff")] public bool isBuff;

            [LabelWidth(100), VerticalGroup("Buff"), ShowIf("@isBuff")] public eBuffType startBuffType;
            [LabelWidth(100), VerticalGroup("Buff"), ShowIf("@isBuff")] public eStat startBuff;
            [LabelWidth(100), VerticalGroup("Buff"), ShowIf("@isBuff")] public float value;
            [LabelWidth(100), VerticalGroup("Buff"), ShowIf("@isBuff")] public eSkillDuration durationType;
            [LabelWidth(100), VerticalGroup("Buff"), ShowIf("@isBuff && durationType == eSkillDuration.Time")] public float time;

            [LabelWidth(100), VerticalGroup("SpawnAngle")] public bool randomAngle;
            [LabelWidth(100), VerticalGroup("SpawnAngle"), ShowIf("@randomAngle == false")] public float angle = 0;
            [LabelWidth(100), VerticalGroup("SpawnAngle"), ShowIf("@randomAngle")] public float minAngle = 0;
            [LabelWidth(100), VerticalGroup("SpawnAngle"), ShowIf("@randomAngle")] public float maxAngle = 360;

            [LabelWidth(100), VerticalGroup("SpawnRadius")] public bool randomRadius;
            [LabelWidth(100), VerticalGroup("SpawnRadius"), ShowIf("@randomRadius == false")] public float radius = 15;
            [LabelWidth(100), VerticalGroup("SpawnRadius"), ShowIf("@randomRadius")] public float minRadius = 10;
            [LabelWidth(100), VerticalGroup("SpawnRadius"), ShowIf("@randomRadius")] public float maxRadius = 15;
        }
        [Serializable]
        public class SpawnNormalInfo : SpawnInfo
        {
            [MinValue(1)] public int spawnMaxCount = 1;
            [MinValue(0)] public float coolTime;
        }
        [Serializable]
        public class SpawnWaveInfo : SpawnInfo
        {
            [MinValue(1)] public int waveNumber = 1;
            [MinValue(1)] public int spawnCount = 1;
        }
        #endregion

        #region StageType
        [Serializable]
        public abstract class StageType
        {

        }

        [Serializable]
        public class NormalStage : StageType
        {
            public float playTime;

            [TableList] public List<SpawnNormalInfo> spawnInfoList = new List<SpawnNormalInfo>();
        }
        [Serializable]
        public class WaveStage : StageType
        {
            public int waveCount;

            [TableList] public List<SpawnWaveInfo> spawnInfoList = new List<SpawnWaveInfo>();
        }
        #endregion

        #region PreviewUnit
        [Serializable]
        public class PreviewUnit
        {
            [NonSerialized, FoldoutGroup("���� �̸�����"), LabelText("���� �ε���"), ShowInInspector, LabelWidth(150)] public int unitSeed;
            [NonSerialized, FoldoutGroup("���� �̸�����"), LabelText("���� ����"), ShowInInspector, LabelWidth(150)] public int unitLevel;
            [NonSerialized] private List<UnitData> unitDataList;
            [NonSerialized, FoldoutGroup("���� �̸�����"), LabelText("���� ����"), ReadOnly, ShowInInspector, ShowIf("@IsNullInfo() == false"), LabelWidth(150)] public UnitData unitData;

            [FoldoutGroup("���� �̸�����"), Button("���� ������ �ҷ�����", ButtonSizes.Small), InfoBox("�����Ͱ� �����ϴ�. ������ �ҷ�������.", InfoMessageType.Error, "IsNullData")]
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

            [FoldoutGroup("���� �̸�����"), Button("���� ���� Ȯ��", ButtonSizes.Small), InfoBox("������ �����ϴ�. �ε����� Ȯ�����ּ���.", InfoMessageType.Error, "IsNullInfo")]
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
        #endregion
    }



}