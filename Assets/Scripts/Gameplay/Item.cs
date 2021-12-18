using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item : Entity
{
    public string ItemId;
    public string DescriptionId;
    public ItemType ItemType;
    public EquipType equipType;
    public int ItemRare;
    public int Count;
    public bool IsStack;
    public int MaxStack;
    public Sprite Sprite;
    [SerializeField] private List<Collider> colliders = new List<Collider>();
    [SerializeField] private List<Renderer> rendrers = new List<Renderer>();
    [SerializeField] private List<ItemStats> itemStats = new List<ItemStats>();
    [SerializeField] private Rigidbody rigidbody;

    public Rigidbody Rigidbody => rigidbody;

    public GameObject Prefab;

    protected override void Start()
    {

    }
    #if UNITY_EDITOR
    public void LoadData()
    {
        ItemDataSO data = DatabaseManager.Instance.GetItemData(ItemId);
        
        if (!data)
        {
            Debug.LogError("Cant find item with id: " + ItemId);
            return;
        }

        gameObject.name = data.ItemId;
        DescriptionId = data.DescriptionId;
        ItemType = data.ItemType;
        equipType = data.equipType;
        ItemRare = data.ItemRare;
        Count = 1;
        Sprite = data.Sprite;
        gameObject.layer = LayerMask.NameToLayer("Triggered");
        
        itemStats.Clear();
        
        foreach (var stat in data.ItemStats)
        {
            ItemStats ststs = new ItemStats();
            ststs.StatName = stat.StatName;
            ststs.StatValue = stat.StatValue;
            itemStats.Add(ststs);
        }

        colliders.Clear();
        
        foreach (var collider in GetComponents<Collider>())
        {
            colliders.Add(collider);
        }

        rendrers.Clear();
        
        foreach (var rend in GetComponents<Renderer>())
        {
            rendrers.Add(rend);
        }

        rigidbody = GetComponent<Rigidbody>();
        Prefab = gameObject;
        
        
        string localPath = "";

        switch (ItemType)
        {
            case ItemType.Equip:
                localPath = "Assets/Prefabs/Items/" + gameObject.name + ".prefab";
                break;
            case ItemType.Loot:
                localPath = "Assets/Prefabs/Items/" + gameObject.name + ".prefab";
                break;
            case ItemType.Resource:
                localPath = "Assets/Prefabs/Resources/" + gameObject.name + ".prefab";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        // Create the new Prefab.
        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
        Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(localPath);
        data.Prefab = Prefab;
    }
    #endif
    public Item CloneItem(Item item)
    {
        
        ItemId = item.ItemId;
        DescriptionId = item.DescriptionId;
        equipType = item.equipType;
        ItemRare = item.ItemRare;
        Count = item.Count;
        IsStack = item.IsStack;
        MaxStack = item.MaxStack;
        Sprite = item.Sprite;
        colliders.AddRange(item.colliders);
        rendrers.AddRange(item.rendrers);
        itemStats.AddRange(item.itemStats);
        rigidbody = item.rigidbody;
        Prefab = item.Prefab;
        return this;
    }
    
    public void Init()
    {

    }

    public void Visualize(bool state)
    {
        foreach(var c in colliders)
        {
            c.enabled = state;
        }

        foreach (var c in rendrers)
        {
            c.enabled = state;
        }
        
        if (state)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
        else
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }

    }

    public override string GetName()
    {
        return ItemId;
    }

    public override int GetRare()
    {
        return ItemRare;
    }

    public override int GetCount()
    {
        return Count;
    }

    public void CollectItem(Transform inventory)
    {

    }

    public float GetStat(string statName)
    {
        float value = 0;
        foreach(var s in itemStats)
        {
            if(s.StatName == statName)
            {
                value = s.StatValue;
                return value;
            }
        }

        return value;
    }

    public override Sprite GetSprite()
    {
        return Sprite;
    }

    [System.Serializable]
    public class ItemStats
    {
        public string StatName;
        public float StatValue;
    }
}

public enum ItemType
{
    Equip,Loot,Resource,Item
}

public enum EquipType
{
    Backpack, none
}
#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Item myScript = (Item)target;
        if(GUILayout.Button("Load From Database"))
        {
            myScript.LoadData();
        }
    }
}
#endif


