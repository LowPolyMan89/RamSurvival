using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DatabaseManager :MonoBehaviour
{
    [SerializeField] private ItemDBSO _itemsDatabase;
    public static readonly Dictionary<string, ItemDataSO> ItemsData = new Dictionary<string, ItemDataSO>();
    [SerializeField] private OtherDataSO _otherData;
    public static DatabaseManager Instance;

    public OtherDataSO OtherData => _otherData;

    private void Start()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(this);
        }
    }

    public void LoadItemsDatabase()
    { 
        foreach (var data in _itemsDatabase.ItemsData)
        {
            string id = data.ItemId.ToLower();
            data.DescriptionId = data.DescriptionId.ToLower();
            data.ItemId = id;
           
            ItemsData.Add(id, data);
        }
       
        Debug.Log("Item data loaded!");
    }
    
    
    public ItemDataSO GetItemData(string itemId)
    {
        
        string id = itemId.ToLower();

        if (ItemsData.TryGetValue(id, out var value))
        {
            
        }
        else
        {
            Debug.LogError("Cant find Item in Database: " + id);
        }
        return value;
    }

}
