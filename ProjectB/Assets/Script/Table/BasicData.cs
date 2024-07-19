using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Data
{
    public interface IDataKey<E>
    {
        public E Key { get; }
    }

    public class UnitData : IDataKey<int>
    {
        public int Seed;
        public string Name;
        public eUnitType Type;

        public long hp;
        public long mp;
        public long atk;
        public long def;
        public float acc;
        public float dod;
        public float atkSpd;
        public float moveSpd;
        public float atkRange;
        public float criRate;
        public float criDmg;
        public long exp;
        public Texture2D icon;
        public string modelAssetRef;
        public string animatorAssetRef;
        public int atkIdx;
        public List<int> skillGroup = new List<int>();

        public SkillRecord atkInfo;
        public List<SkillRecord> skillInfoGroup = new List<SkillRecord>();

        public UnitData(Editor.UnitData data)
        {
            Seed = data.Seed;
            Name = data.Name;
            Type = data.Type;

            hp = data.info.hp;
            mp = data.info.mp;
            atk = data.info.atk;
            def = data.info.def;
            acc = data.info.acc;
            dod = data.info.dod;
            atkSpd = data.info.atkSpd;
            moveSpd = data.info.moveSpd;
            atkRange = data.info.atkRange;
            criRate = data.info.criRate;
            criDmg = data.info.criDmg;
            exp = data.info.exp;
            icon = data.info.icon;
            modelAssetRef = data.info.modelAssetRef;
            animatorAssetRef =data.info.animatorAssetRef;
            atkInfo = TableManager.Instance.skillTable.GetRecord(data.info.atkInfoIdx);
            for (int i = 0; i < data.info.skillInfoIdxGroup.Length; i++)
            {
                skillInfoGroup.Add(TableManager.Instance.skillTable.GetRecord(data.info.skillInfoIdxGroup[i]));
            }
        }
        public int Key => Seed;
    }
    public class StageData : IDataKey<int>
    {
        public readonly int Seed;
        public readonly string Name;
        public readonly eStageType Type;
        public readonly StageInfo stageInfo;

        public StageData(Editor.StageData data)
        {
            Seed = data.Seed;
            Name = data.Name;
            Type = data.stageType;
            switch (data.getStageType)
            {
                case Editor.StageData.NormalStage normalStage:
                    stageInfo = new NormalStage(normalStage);
                    break;
                case Editor.StageData.WaveStage waveStage:
                    stageInfo = new WaveStage(waveStage);
                    break;
            }
        }

        #region StageType
        [Serializable]
        public class StageInfo
        {
            public StageInfo(Editor.StageData.StageType stageType)
            {

            }
        }

        public class NormalStage : StageInfo
        {
            public float playTime;
            public List<SpawnNormalInfo> spawnInfoList = new List<SpawnNormalInfo>();

            public NormalStage(Editor.StageData.NormalStage stageType) : base(stageType)
            {
                playTime = stageType.playTime;
                spawnInfoList.Clear();
                for (int i = 0; i < stageType.spawnInfoList.Count; i++)
                {
                    var spawnInfo = new SpawnNormalInfo(stageType.spawnInfoList[i]);
                    spawnInfoList.Add(spawnInfo);
                }
            }
        }
        public class WaveStage : StageInfo
        {
            public int waveCount;
            public List<SpawnWaveInfo> spawnInfoList = new List<SpawnWaveInfo>();

            public WaveStage(Editor.StageData.WaveStage stageType) : base(stageType)
            {
                waveCount = stageType.waveCount;
                spawnInfoList.Clear();
                for (int i = 0; i < stageType.spawnInfoList.Count; i++)
                {
                    var spawnInfo = new SpawnWaveInfo(stageType.spawnInfoList[i]);
                    spawnInfoList.Add(spawnInfo);
                }
            }
        }
        #endregion

        #region SpawnInfo
        public abstract class SpawnInfo
        {
            public readonly int monsterSeed;
            public readonly int monsterLevel;
            public readonly float startDelay;
            public readonly bool isBuff;

            public readonly eBuffType startBuffType;
            public readonly eStat startBuff;
            public readonly eSkillDuration durationType;

            public readonly bool randomAngle;
            public readonly float angle;
            public readonly float minAngle;
            public readonly float maxAngle;

            public readonly bool randomRadius;
            public readonly float radius;
            public readonly float minRadius;
            public readonly float maxRadius;

            public SpawnInfo(Editor.StageData.SpawnInfo spawnInfo)
            {
                monsterSeed = spawnInfo.monsterSeed;
                monsterLevel = spawnInfo.monsterLevel;
                startDelay = spawnInfo.startDelay;
                isBuff = spawnInfo.isBuff;
                startBuffType = spawnInfo.startBuffType;
                startBuff = spawnInfo.startBuff;
                durationType = spawnInfo.durationType;
                randomAngle = spawnInfo.randomAngle;
                angle = spawnInfo.angle;
                minAngle = spawnInfo.minAngle;
                maxAngle = spawnInfo.maxAngle;
                randomRadius = spawnInfo.randomRadius;
                radius = spawnInfo.radius;
                minRadius = spawnInfo.minRadius;
                maxRadius = spawnInfo.maxRadius;
            }
        }
        public class SpawnNormalInfo : SpawnInfo
        {
            public readonly int spawnMaxCount;
            public readonly float coolTime;
            public SpawnNormalInfo(Editor.StageData.SpawnNormalInfo spawnInfo) : base(spawnInfo)
            {
                spawnMaxCount = spawnInfo.spawnMaxCount;
                coolTime = spawnInfo.coolTime;
            }
        }
        public class SpawnWaveInfo : SpawnInfo
        {
            public readonly int waveNumber;
            public readonly int spawnCount;
            public SpawnWaveInfo(Editor.StageData.SpawnWaveInfo spawnInfo) : base(spawnInfo)
            {
                waveNumber = spawnInfo.waveNumber;
                spawnCount = spawnInfo.spawnCount;
            }
        }
        #endregion
        public int Key => Seed;
    }
}