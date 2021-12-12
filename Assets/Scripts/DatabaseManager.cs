using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DatabaseManager
{
    private static ItemDBSO _itemsDatabase;
    public static readonly Dictionary<string, ItemDataSO> ItemsData = new Dictionary<string, ItemDataSO>();
    public static OtherDataSO OtherData;
    
    [MenuItem("DatabaseManager/LoadDatabase")]
    public static void LoadItemsDatabase()
    {
       var result = AssetDatabase.FindAssets("ItemDatabase");
       var path = AssetDatabase.GUIDToAssetPath(result[0]);
       
       var otherGUID = AssetDatabase.FindAssets("OtherDataSO");
       var otherPath = AssetDatabase.GUIDToAssetPath(otherGUID[0]);
       
       _itemsDatabase = (ItemDBSO)AssetDatabase.LoadAssetAtPath(path, typeof(ItemDBSO));
       OtherData = (OtherDataSO)AssetDatabase.LoadAssetAtPath(otherPath, typeof(OtherDataSO));

       foreach (var data in _itemsDatabase.ItemsData)
       {
           string id = data.ItemId.ToLower();
           data.DescriptionId = data.DescriptionId.ToLower();
           data.ItemId = id;
           
           ItemsData.Add(id, data);
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
        
        string id = itemId.ToLower();
        
        if (_itemsDatabase == null)
        {
            LoadItemsDatabase();
            Test();
        }

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
