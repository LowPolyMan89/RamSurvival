using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour
{
    public CraftSheme Sheme;
    public CraftController _craftController;
    
    private void Start()
    {
        _craftController = new CraftController();
        _craftController.Init();
    }
    
    public void OpenCraft()
    {
        _craftController.OpenCraft(Sheme, Player.Instance.PlayerInventory, this);
    }

    public void StartCraft(CrafterUi.BlueprintItemsCollection blueprintItemsCollection)
    {
        _craftController.StartCraft(blueprintItemsCollection);
    }

    public void OpenCraft(CraftSheme sheme, Inventory inventory, Crafter crafter)
    {
        _craftController.OpenCraft(sheme, inventory, crafter);
    }
    
}
