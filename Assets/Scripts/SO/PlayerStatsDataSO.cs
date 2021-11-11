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
}
