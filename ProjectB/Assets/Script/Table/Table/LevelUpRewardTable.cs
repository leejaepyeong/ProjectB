using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelUpRewardRecord : RecordBase
{
    public int groupIdx;
    public eLevelUpReward itemType;
    public int itemIndex;
    public int rate;

    public override void LoadExcel(Dictionary<string, string> _data)
    {
        base.LoadExcel(_data);
        groupIdx = FileUtil.Get<int>(_data, "GroupId");
        itemType = FileUtil.Get<eLevelUpReward>(_data, "ItemType");
        itemIndex = FileUtil.Get<int>(_data, "ItemIndex");
        rate = FileUtil.Get<int>(_data, "Rate");
    }
}
public class LevelUpRewardTable : TTableBase<LevelUpRewardRecord>
{
    private Dictionary<int, List<LevelUpRewardRecord>> dicLevelUpReward = new Dictionary<int, List<LevelUpRewardRecord>>();
    public LevelUpRewardTable(ClassFileSave save, string path) : base(save, path)
    {

    }

    public override void Load()
    {
        base.Load();
        dicLevelUpReward.Clear();
        for (int i = 0; i < getRecordList.Count; i++)
        {
            if (dicLevelUpReward.ContainsKey(getRecordList[i].groupIdx) == false)
                dicLevelUpReward.Add(getRecordList[i].groupIdx, new List<LevelUpRewardRecord>());
            dicLevelUpReward[getRecordList[i].groupIdx].Add(getRecordList[i]);
        }
    }

    public List<LevelUpRewardRecord> GetLevelUpRewardList(int groupId, int rewardCount)
    {
        if(dicLevelUpReward.TryGetValue(groupId, out var rewardList) == false)
        {
            Debug.LogError("No LevelUp Reward");
            return null;
        }

        if (rewardList.Count <= rewardCount)
            return rewardList;

        List<LevelUpRewardRecord> reward = new List<LevelUpRewardRecord>();
        List<LevelUpRewardRecord> temp = new List<LevelUpRewardRecord>();
        int totalRate = 0;
        for (int i = 0; i < rewardList.Count; i++)
        {
            temp.Add(rewardList[i]);
            totalRate += rewardList[i].rate;
        }

        int tempRate = 0;
        while(reward.Count < rewardCount)
        {
            int random = Random.Range(1, totalRate);
            for (int i = 0; i < temp.Count; i++)
            {
                tempRate += temp[i].rate;
                if (random < tempRate)
                {
                    reward.Add(temp[i]);
                    totalRate -= temp[i].rate;
                    temp.Remove(temp[i]);
                    break;
                }
            }
        }

        return reward;
    }
}
