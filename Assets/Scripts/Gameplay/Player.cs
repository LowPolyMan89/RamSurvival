using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory PlayerInventory;
    public static Player Instance;
    public UiInventory UiInventory;
    public PlayerStats PlayerStats;
    public Item EqippedBackpack;

    public Transform dropPoint;

    public void EqipBackpack(Item backpackItem)
    {
        EqippedBackpack = backpackItem;
    }
    
    private void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }
}
