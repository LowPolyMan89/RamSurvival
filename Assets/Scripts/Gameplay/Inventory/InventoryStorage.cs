using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStorage : Entity
{
    
    [SerializeField] private InventoryStorageUI _inventoryStorageUI;

    public InventoryStorageUI InventoryStorageUI => _inventoryStorageUI;

 


    protected override void Start()
    {
        _inventoryStorageUI = FindObjectOfType<InventoryStorageUI>();
    }
}
