using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController
{
    public CraftSheme Sheme;
    public Inventory InventoryToGetItems;
    public readonly Queue<CraftProcess> CraftProcesses = new Queue<CraftProcess>();

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
        Player.Instance.PlayerStats.Energy -= blueprintItemsCollection.Energy;

        foreach (var removeitem in blueprintItemsCollection.Items)
        {
            InventoryToGetItems.MassRemoveItem(removeitem.Item.ItemId, removeitem.Count);
        }
        
        CraftProcesses.Enqueue(new CraftProcess(blueprintItemsCollection.Time, blueprintItemsCollection.OutputItemId, blueprintItemsCollection.OutputItemValue));
    }

    private void CraftTick()
    {
        if(CraftProcesses.Count < 1) return;
        CraftProcess process = CraftProcesses.Peek();
        process.CurrentTime += 1;
        if (process.CurrentTime >= process.CraftTimeMax)
        {
            process.CurrentTime = process.CraftTimeMax;
        }
    }

    public void CollectItem()
    {
        CraftProcess process = CraftProcesses.Peek();
        InventoryToGetItems.AddItem(process.OutputItem, process.OutputValue);
        CraftProcesses.Dequeue();
    }

    public void DeInit()
    {
        EventManager.Instance.OnTimerSecondAction -= CraftTick;
    }
}
[System.Serializable]
public class CraftProcess
{
    public CraftProcess(float craftTime, string outputItem, int outputValue)
    {
        CraftTimeMax = craftTime;
        OutputItem = outputItem;
        OutputValue = outputValue;
    }

    public float CurrentTime;
    public float CraftTimeMax;
    public string OutputItem;
    public int OutputValue;
}
