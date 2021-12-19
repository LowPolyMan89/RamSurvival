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
    public Localization Localization;

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

    public void LoadLocalizationData()
    {

        LocalizationData localization;
        
        var textFile = Resources.Load<TextAsset>("Text/Localization");
        
        if (textFile)
        {
            localization = JsonUtility.FromJson<LocalizationData>(textFile.text);
        }
        else
        {
            Debug.LogError("No Localization File!");
            return;
        }
        
        var loc = new Dictionary<string, List<string>>();
        
        if (loc == null) throw new ArgumentNullException(nameof(loc));

        foreach (var l in localization.Locals)
        {
            loc.Add(l.LocalizationObjectID, l.Local);
        }

        Localization.Data = loc;
        
        Debug.Log("LoadLocalization Data loaded!");
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

[System.Serializable]
public class Localization
{
     public  Dictionary<string, List<string>> Data = new Dictionary<string, List<string>>();

     public string GetLocalization(string id)
     {
         int lang = 0;
         
         #if UNITY_EDITOR
         lang = 0;
         #else
         switch (Application.systemLanguage)
         {
             case SystemLanguage.Russian:
                 lang = 0;
                 break;
             case SystemLanguage.English:
                 lang = 1;
                 break;
             default:
                 break;
         }
         #endif
         
         string text = id.ToLower();

         if (Data.TryGetValue(id, out var value))
         {
             if (value.Count < lang)
                 return text;
             text = value[lang];
         }

         return text;
     }
}
