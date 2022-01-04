using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private string building_ID;
    public int Current_lvl; 
    [SerializeField] private List<GameObject> buildings_levels = new List<GameObject>();
    [SerializeField] private BuildingUpgradeProcess upgradeProcess;
    [SerializeField] private BuildingInventory buildingInventory;

    public BuildingInventory Inventory => buildingInventory;

    public BuildingUpgradeProcess UpgradeProcess => upgradeProcess;

    private void Start()
    {
        UpgradeBuilding(0);
        UpgradeProcess.CurrentBuilding = this;
        buildingInventory.Init(upgradeProcess, this);
    }

    private int UpgradeBuilding(int addLevel)
    {
        int Level = addLevel + Current_lvl;
        
        foreach (var buildingsLevel in buildings_levels)
        {
            buildingsLevel.SetActive(false);
        }
        buildings_levels[Level].SetActive(true);

        return Level;
    }

    public void Upgrade()
    {
        UpgradeBuilding(1);
    }

    [System.Serializable]
    public class BuildingInventory
    {
        public Dictionary<string, CurrentAndNeedValue> Items = new Dictionary<string, CurrentAndNeedValue>();
        private Building currentBuilding;
        private BuildingUpgradeProcess currentProcess;
        public void Init(BuildingUpgradeProcess process, Building building)
        {

            currentBuilding = building;
            currentProcess = process;
            
            foreach (var v in process.UpgradeData.ItemsToUpgrade)
            {
                Items.Add(v.item.ItemId, new CurrentAndNeedValue(){Current = 0, Need = v.ItemValue});
            }
        }
    

    public int AddItemAndReturnIfFull(string id, int value)
        {
            CurrentAndNeedValue val = new CurrentAndNeedValue();
            int count = value;
            int needValue = 0;
            
            if (Items.TryGetValue(id, out val))
            {
                needValue = val.Need - val.Current;
                
                if (needValue == 0)
                {
                    UIController.Instance.UiBuildings.Refresh();
                    return count;
                }
                
                if (needValue >= count)
                {
                    val.Current += count;
                    Items[id] = val;
                    UIController.Instance.UiBuildings.Refresh();
                    return 0;
                }
                else
                {
                    int valueToReturn = count - needValue;
                    val.Current = val.Need;
                    Items[id] = val;
                    UIController.Instance.UiBuildings.Refresh();
                    return valueToReturn;
                }
            }
            
            UIController.Instance.UiBuildings.Refresh();
            
            return count;
            
        }
    }
    
    public struct CurrentAndNeedValue
    {
        public int Current;
        public int Need;
    }
    
    [System.Serializable]
    public class BuildingUpgradeProcess
    {
        public bool IsProcess;
        public float ProcesTime;
        public Building CurrentBuilding;
        [SerializeField] private BuildingUpgradeSO _upgradeData;

        public BuildingUpgradeSO UpgradeData => _upgradeData;

        private void ProcessTick()
        {
            if(!IsProcess) return;
        }
        
    }
}
