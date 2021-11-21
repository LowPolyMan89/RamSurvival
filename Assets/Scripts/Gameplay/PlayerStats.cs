using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float hitPoint;
    [SerializeField] private float food;
    [SerializeField] private float energy;
    [SerializeField] private float minimumMass;
    [SerializeField] private int minimumCells;
    [SerializeField] private PlayerStatsDataSO _playerStatsDataSo;
    private void Awake()
    {
        hitPoint = _playerStatsDataSo.HitPoint;
        food = _playerStatsDataSo.Food;
        minimumMass = _playerStatsDataSo.MinimalInventoryCapacity;
        minimumCells = _playerStatsDataSo.MimimalInventoryCells;
    }

    public float MinimumMass => minimumMass;

    public int MinimumCells => minimumCells;
}
