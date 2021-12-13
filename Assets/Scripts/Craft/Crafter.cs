using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : Entity
{
    public CraftSheme Sheme;
    public CraftController _craftController;
    public int CraftSlots;
    public CraftersDataSO Data;
    public int Level = 1;
    
    private void Start()
    {
        _craftController = new CraftController();
        _craftController.Init();
        CraftSlots = Mathf.Clamp(CraftSlots, 0, 5);
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
