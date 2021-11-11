using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStorage : Inventory
{
    [SerializeField] private float capacity;

    public float Capacity { get => capacity; set => capacity = value; }

    public void Create()
    {

    }
}
