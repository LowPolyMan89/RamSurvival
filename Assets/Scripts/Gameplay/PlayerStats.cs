using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHitPoint;
    [SerializeField] private float currentHitPoint;
    [FormerlySerializedAs("food")] [SerializeField] private float currentFood;
    [SerializeField] private float maxFood;
    [FormerlySerializedAs("energy")] [SerializeField] private float currentEnergy;
    [SerializeField] private float maxEnergy;
    public BuffController BuffController;

    public float MAXHitPoint => GetMaxHitPoints();

    public float CurrentHitPoint => currentHitPoint;

    public float CurrentFood => currentFood;

    public float MAXFood => maxFood;

    public float CurrentEnergy => currentEnergy;

    public float MAXEnergy => maxEnergy;

    public int CurrentXp => currentXp;

    public int CurrentLvl => currentLvl;

    public PlayerStatsDataSO PlayerStatsDataSo => _playerStatsDataSo;

    [SerializeField] private float minimumMass;
    [SerializeField] private int minimumCells;
    [SerializeField] private int currentXp;
    [SerializeField] private int currentLvl = 1;

    
    public float MinimumMass => minimumMass;
    public int MinimumCells => minimumCells;

    [SerializeField] private PlayerStatsDataSO _playerStatsDataSo;

    public void Init()
    {
        maxHitPoint = _playerStatsDataSo.HitPoint;
        currentHitPoint = maxHitPoint; //TOREMOVE
        currentFood = _playerStatsDataSo.Food;
        maxFood = _playerStatsDataSo.Food;
        maxEnergy = _playerStatsDataSo.Energy;
        currentEnergy = maxEnergy;
        minimumMass = _playerStatsDataSo.MinimalInventoryCapacity;
        minimumCells = _playerStatsDataSo.MimimalInventoryCells;

        BuffController = new BuffController();
        BuffController.Init();
        EventManager.Instance.OnPlayerGetXPAction += AddExp;
        EventManager.Instance.OnPlayerGetHPAction += AddHP;
        EventManager.Instance.OnPlayerGetFoodAction += AddFood;
        EventManager.Instance.OnPlayerGetEnergyAction += AddEnergy;
        EventManager.Instance.OnTimerSecondAction += OneSecondTick;
        
        print("Player stats is ready!");
    }

    [ContextMenu("TestPerkHp")]
    public void TestPerkHp()
    {
        BuffController.CreateNewBuff("add_max_hp_perk_1");
    }
    
    [ContextMenu("TestBuffHp")]
    public void TestBuffHp()
    {
        BuffController.CreateNewBuff("add_max_hp_buff_1");
    }
    
    [ContextMenu("RemovePerkHp")]
    public void RemovePerkHp()
    {
        BuffController.RemoveBuff("add_max_hp_perk_1");
    }
    
    public float GetMaxHitPoints()
    {
        if (BuffController == null) return _playerStatsDataSo.HitPoint;
        
        float hp = BuffController.GetMaxHitPoint(_playerStatsDataSo.HitPoint);
        return hp;
    }
    
    private void OneSecondTick()
    {
        EventManager.Instance.OnPlayerGetEnergy(0.1f);
        EventManager.Instance.OnPlayerGetFood(-0.2f);
    }
    
    public int AddExp(int expToAdd)
    {
        currentXp += expToAdd;
        currentLvl = _playerStatsDataSo.GetPlayerLevel(currentXp);
        return expToAdd;
    }
    
    public float AddHP(float hp)
    {
        currentHitPoint += hp;
        currentHitPoint = Mathf.Clamp(currentHitPoint, 0, maxHitPoint);
        if (currentHitPoint <= 0)
        {
            print("Player is dead!");
        }
        return currentHitPoint;
    }
    
    public float AddFood(float food)
    {
        currentFood += food;
        currentFood = Mathf.Clamp(currentFood, 0, maxFood);
        
        if (currentFood <= 0)
        {
            print("Player is hungry!!");
            EventManager.Instance.OnPlayerGetHP(-5); //TOREMOVE
        }
        
        return currentFood;
    }
    
    public float AddEnergy(float energy)
    {
        currentEnergy += energy;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        
        if (currentEnergy <= 0)
        {
            print("Player is no energy!!");
        }
        
        return currentEnergy;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnPlayerGetXPAction -= AddExp;
        EventManager.Instance.OnPlayerGetHPAction -= AddHP;
        EventManager.Instance.OnPlayerGetFoodAction -= AddFood;
        EventManager.Instance.OnPlayerGetEnergyAction -= AddEnergy;
        EventManager.Instance.OnTimerSecondAction -= OneSecondTick;
    }
}
