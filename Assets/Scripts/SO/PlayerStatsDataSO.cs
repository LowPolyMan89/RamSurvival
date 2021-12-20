using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Data/PlayerStatsData", order = 2)]
public class PlayerStatsDataSO : ScriptableObject
{
    public float HitPoint;
    public float Food;
    public float Energy;
    public int MimimalInventoryCells;
    public float MinimalInventoryCapacity;
    public ExpStats expStats;


    public int GetPlayerLevel(int exp)
    {
        int currexp = exp;
        
        foreach (var stat in expStats.PlayerLevelStats)
        {
            if (stat.ExpToNextLevel > currexp)
            {
                return stat.Level;
            }
        }

        return 1;
    }
    
    [System.Serializable]
    public class ExpStats
    {
        public List<PlayerLevelStat> PlayerLevelStats = new List<PlayerLevelStat>();

        [System.Serializable]
        public class PlayerLevelStat
        {
            public int Level;
            public int ExpToNextLevel;
        }
    }
}
