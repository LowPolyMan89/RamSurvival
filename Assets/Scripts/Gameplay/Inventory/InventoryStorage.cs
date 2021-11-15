using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStorage : Entity
{
    [SerializeField] private float capacity;
    [SerializeField] private int slots;
    [SerializeField] private  List<Item> _items = new List<Item>();
    [SerializeField] private InventoryStorageUI _inventoryStorageUI;

    public InventoryStorageUI InventoryStorageUI => _inventoryStorageUI;

    public float Capacity { get => capacity; set => capacity = value; }

    public int Slots => slots;

    public List<Item> Items => _items;

    protected override void Start()
    {
        _inventoryStorageUI = FindObjectOfType<InventoryStorageUI>();
    }
}
