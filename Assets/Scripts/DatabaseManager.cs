using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DatabaseManager
{
    private static ItemDBSO _itemsDatabase;
    public static readonly Dictionary<string, ItemDataSO> ItemsData = new Dictionary<string, ItemDataSO>();
    
    [MenuItem("DatabaseManager/LoadDatabase")]
    public static void LoadItemsDatabase()
    {
       var result = AssetDatabase.FindAssets("ItemDatabase");
       var path = AssetDatabase.GUIDToAssetPath(result[0]);
       
       _itemsDatabase = (ItemDBSO)AssetDatabase.LoadAssetAtPath(path, typeof(ItemDBSO));

       foreach (var data in _itemsDatabase.ItemsData)
       {
           ItemsData.Add(data.ItemId, data);
       }
       
       Debug.Log("Item data loaded!");
    }

    [MenuItem("DatabaseManager/LoadingTest")]
    public static void Test()
    {
        try
        {
            ItemDataSO value;
            ItemsData.TryGetValue("Wood_T1_name_id", out value);
            if (value)
            {
                Debug.Log(value.DescriptionId);
                Debug.Log("Database Manager is now loaded");
            }
            
        }
        catch (Exception e)
        {
            Debug.LogError("Database Manager is not loaded!");
        }
        
    }

    public static ItemDataSO GetItemData(string itemId)
    {
        if (_itemsDatabase == null)
        {
            LoadItemsDatabase();
            Test();
        }

        if (ItemsData.TryGetValue(itemId, out var value))
        {
            
        }
        else
        {
            Debug.LogError("Cant find Item in Database: " + itemId);
        }
        return value;
    }

    public static Sprite GetItemSprite(string itemId)
    {
        return null;
    }
    
}
