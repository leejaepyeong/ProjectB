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
        [HorizontalGroup("Info", 180, 10), LabelText("스테이지 인덱스"), LabelWidth(100)] public int Seed;
        [HorizontalGroup("Info", 160, 5), LabelWidth(50)] public string Name;
        [HorizontalGroup("Info", 180, 5),LabelText("스테이지 타입"), LabelWidth(80)] public eStageType stageType;
        [HorizontalGroup("Info", 160, 5), LabelText("플레이 시간"), MinValue(0), LabelWidth(80)] public float playTime;

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
            public int monsterSeed;
            public int monsterLevel;
            [MinValue(0)] public float startDelay;
            public bool isBuff;

            [ShowIf("@isBuff")] public eBuffType startBuffType;
            [ShowIf("@isBuff")] public eStat startBuff;
            [ShowIf("@isBuff")] public eSkillDuration durationType;

            public float minAngle = 0;
            public float maxAngle = 360;

            public float minRadius = 10;
            public float maxRadius = 15;
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
            [MinValue(1)] public int waveNumber;
            [MinValue(1)] public int spawnCount;
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
            [NonSerialized, FoldoutGroup("유닛 미리보기"), LabelText("유닛 인덱스"), ShowInInspector, LabelWidth(150)] public int unitSeed;
            [NonSerialized, FoldoutGroup("유닛 미리보기"), LabelText("유닛 레벨"), ShowInInspector, LabelWidth(150)] public int unitLevel;
            [NonSerialized] private List<UnitData> unitDataList;
            [NonSerialized, FoldoutGroup("유닛 미리보기"), LabelText("유닛 정보"), ReadOnly, ShowInInspector, ShowIf("@IsNullInfo() == false"), LabelWidth(150)] public UnitData unitData;

            [FoldoutGroup("유닛 미리보기"), Button("몬스터 데이터 불러오기", ButtonSizes.Small), InfoBox("데이터가 없습니다. 정보를 불러오세요.", InfoMessageType.Error, "IsNullData")]
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

            [FoldoutGroup("유닛 미리보기"), Button("몬스터 정보 확인", ButtonSizes.Small), InfoBox("정보가 없습니다. 인덱스를 확인해주세요.", InfoMessageType.Error, "IsNullInfo")]
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