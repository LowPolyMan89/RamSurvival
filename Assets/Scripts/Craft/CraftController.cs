using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController
{
    public CraftSheme Sheme;
    public Inventory InventoryToGetItems;

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
        
        InventoryToGetItems.AddItem(blueprintItemsCollection.OutputItemId, blueprintItemsCollection.OutputItemValue);
    }
}
