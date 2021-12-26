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
    [SerializeField] private List<BuffView> ActiveBuffs = new List<BuffView>();

    public float MAXHitPoint => GetMaxHitPoints();

    public float CurrentHitPoint => currentHitPoint;

    public float CurrentFood => currentFood;

    public float MAXFood => GetMaxFood();

    public float CurrentEnergy => currentEnergy;

    public float MAXEnergy => maxEnergy;

    public int CurrentXp => currentXp;

    public int CurrentLvl => currentLvl;

    public float MaxMass => GetMaxMass();

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

    
    [ContextMenu("xp_perk_T1")]
    public void xp_perk_T1()
    {
        Buff b = BuffController.CreateNewBuff("xp_perk_T1");
        BuffView view = new BuffView();
        view.BuffId = b.GetId();
        view.BuffTime = b.GetTime();
        view.Buff = b;
        ActiveBuffs.Add(view);
    }
    
    [ContextMenu("mass_buff_T1")]
    public void mass_buff_T1()
    {
        Buff  b = BuffController.CreateNewBuff("mass_buff_T1");
        BuffView view = new BuffView();
        view.BuffId = b.GetId();
        view.BuffTime = b.GetTime();
        view.Buff = b;
        ActiveBuffs.Add(view);
    }
        
    [ContextMenu("TestPerkHp")]
    public void TestPerkHp()
    { 
        Buff  b = BuffController.CreateNewBuff("add_max_hp_perk_1");
       BuffView view = new BuffView();
       view.BuffId = b.GetId();
       view.BuffTime = b.GetTime();
       view.Buff = b;
       ActiveBuffs.Add(view);
    }
    
    [ContextMenu("TestBuffHp")]
    public void TestBuffHp()
    {
        Buff  b = BuffController.CreateNewBuff("add_max_hp_buff_1");
        BuffView view = new BuffView();
        view.BuffId = b.GetId();
        view.BuffTime = b.GetTime();
        view.Buff = b;
        ActiveBuffs.Add(view);
    }
    
    [ContextMenu("TestfoodBuffT1")]
    public void TestfoodBuffT1()
    {
        Buff  b = BuffController.CreateNewBuff("food_buff_T1");
        BuffView view = new BuffView();
        view.BuffId = b.GetId();
        view.BuffTime = b.GetTime();
        view.Buff = b;
        ActiveBuffs.Add(view);
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
    
    public float GetMaxMass()
    {
        float mass = Player.Instance.PlayerInventory.GetMaxInventoryMass();
        if (BuffController == null) return mass;
        
        mass = BuffController.GetMaxMass(mass);
        return mass;
    }
    
    public float GetMaxFood()
    {
        if (BuffController == null) return _playerStatsDataSo.Food;
        
        float hp = BuffController.GetMaxFood(_playerStatsDataSo.Food);
        return hp;
    }
    
    private void OneSecondTick()
    {
        EventManager.Instance.OnPlayerGetEnergy(0.1f);
        EventManager.Instance.OnPlayerGetFood(-0.2f);

        if (currentHitPoint > BuffController.GetMaxHitPoint(maxHitPoint))
        {
            currentHitPoint = BuffController.GetMaxHitPoint(maxHitPoint);
        }
        
        foreach (var b in ActiveBuffs)
        {
            b.Update();
        }

        ActiveBuffs.RemoveAll(item => item.Buff.IsFinish == true);
    }
    
    public int AddExp(int expToAdd)
    {
        currentXp += (int)BuffController.GetBuffedExpValue(expToAdd);
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
    
    [System.Serializable]
    public class BuffView
    {
        public string BuffId;
        public float BuffTime;
        public Buff Buff;

        public void Update()
        {
            BuffTime = Buff.GetTime();
        }
    }
}
