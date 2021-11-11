using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float hitPoint;
    [SerializeField] private float food;
    [SerializeField] private float energy;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStatsDataSO playerStatsData;
    [SerializeField] private PlayerBackpackDataSO playerBackpackData;
    [SerializeField] private PlayerMultitoolDataSO playerMultitoolData;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private Transform backpackPoint;

    public static PlayerStats instance;

    public float HitPoint { get => hitPoint; }
    public float Food { get => food; }
    public float Energy { get => energy; }
    public PlayerStatsDataSO PlayerStatsData { get => playerStatsData; set => playerStatsData = value; }
    public PlayerBackpackDataSO PlayerBackpackData { get => playerBackpackData; set => playerBackpackData = value; }
    public PlayerMultitoolDataSO PlayerMultitoolData { get => playerMultitoolData; set => playerMultitoolData = value; }
    public Transform DropPoint { get => dropPoint; }
    public Transform BackpackPoint { get => backpackPoint; }
    public Inventory Inventory { get => inventory; set => inventory = value; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        hitPoint = PlayerStatsData.HitPoint;
        food = PlayerStatsData.Food;
        energy = PlayerStatsData.Energy;
    }

    public float GetInventoryCapacity()
    {
        float cap = 0;

        cap = PlayerStatsData.MinimalInventoryCapacity + (playerBackpackData != null ? playerBackpackData.MaxCapacity : 0);

        return cap;
    }

    public int GetInventoryCellsCount()
    {
        int cap = 0;

        cap = PlayerStatsData.MimimalInventoryCells + (playerBackpackData != null ? playerBackpackData.MaxSlots : 0);

        return cap;
    }
}
