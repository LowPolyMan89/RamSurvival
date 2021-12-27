using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory PlayerInventory;
    public static Player Instance;
    public UiInventory UiInventory;
    public PlayerStats PlayerStats;
    public ItemDataSO EqippedBackpack;
    public Crafter playerCrafter;
    public Transform dropPoint;

    public void EqipBackpack([CanBeNull] string backpackItemId)
    {
        EqippedBackpack = string.IsNullOrEmpty(backpackItemId) ? null : DatabaseManager.Instance.GetItemData(backpackItemId);
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

    public void Init()
    {
        PlayerStats.Init();
    }
}
