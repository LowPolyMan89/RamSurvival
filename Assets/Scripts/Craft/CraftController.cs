using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController
{
    public CraftSheme Sheme;
    public Inventory InventoryToGetItems;
    public readonly List<CraftProcess> CraftProcesses = new List<CraftProcess>();

    public void Init()
    {
        EventManager.Instance.OnTimerSecondAction += CraftTick;
    }

    public void OpenCraft(CraftSheme sheme, Inventory inventoryToGetItems, Crafter crafter)
    {
        Sheme = sheme;
        InventoryToGetItems = inventoryToGetItems;
    }

    public void StartCraft(CrafterUi.BlueprintItemsCollection blueprintItemsCollection)
    {
        float nexttime = 0;
        
        EventManager.Instance.OnPlayerGetEnergy(-blueprintItemsCollection.Energy);

        foreach (var removeitem in blueprintItemsCollection.Items)
        {
            InventoryToGetItems.MassRemoveItem(removeitem.Item.ItemId, removeitem.Count);
        }
        UIController.Instance.CrafterUi.Refresh();

        if (CraftProcesses.Count > 0)
        {
            foreach (var p in CraftProcesses)
            {
                nexttime += Sheme.GetBlueprint(p.OutputItem).CraftTimeInSeconds;
            }
        }
        
        CraftProcesses.Add(new CraftProcess(blueprintItemsCollection.Time + nexttime, blueprintItemsCollection.OutputItemId, blueprintItemsCollection.OutputItemValue, blueprintItemsCollection.exp));
    }

    private void CraftTick()
    {
        if(CraftProcesses.Count < 1) return;
        
        foreach (var process in CraftProcesses)
        {
            process.Tick(1);;
            if (process.CurrentTime >= process.CraftTimeMax)
            {
                process.CurrentTime = process.CraftTimeMax;
            }
        }
    }

    public void CollectItem()
    {
        CraftProcess process = CraftProcesses[0];
        InventoryToGetItems.AddItem(process.OutputItem, process.OutputValue);
        CraftProcesses.Remove(process);
    }
    

    public void DeInit()
    {
        EventManager.Instance.OnTimerSecondAction -= CraftTick;
    }
}
[System.Serializable]
public class CraftProcess
{
    public CraftProcess(float craftTime, string outputItem, int outputValue, int exp)
    {
        CraftTimeMax = craftTime;
        OutputItem = outputItem;
        OutputValue = outputValue;
        Exp = exp;
    }
    
    public bool IsComplite = false;
    public float CurrentTime;
    public float CraftTimeMax;
    public string OutputItem;
    public int OutputValue;
    public int Exp;

    public void Tick(float time)
    {
        CurrentTime += time;
        if (CurrentTime >= CraftTimeMax)
        {
            if (!IsComplite)
            {
                EventManager.Instance.AddLog(3,
                    "Завершено:  " + DatabaseManager.Instance.Localization.GetLocalization(OutputItem), Color.green);
            }
            IsComplite = true;
            
        }
    }
}
