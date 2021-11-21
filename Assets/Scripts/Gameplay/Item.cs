using System.Collections;
using System.Collections.Generic;
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
    Equip,Loot,Resource
}

public enum EquipType
{
    Backpack, none
}


