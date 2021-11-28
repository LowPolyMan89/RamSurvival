using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController
{
    public CraftSheme Sheme;
    public Inventory InventoryToGetItems;

    public void OpenCraft(CraftSheme sheme, Inventory inventoryToGetItems)
    {
        Sheme = sheme;
        InventoryToGetItems = inventoryToGetItems;
        UIController.Instance.OpenCraftPanel(Sheme, InventoryToGetItems);
    }
}
