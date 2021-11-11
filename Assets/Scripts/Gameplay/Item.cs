using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public string ItemId;
    public ItemType ItemType;
    public EquipType equipType;
    public int ItemRare;
    public int Count;
    public bool IsStack;
    public Sprite Sprite;
    public ItemDataSO itemDataSO;
    public PlayerBackpackDataSO playerBackpackData;
    public bool isVisualized;
    [SerializeField] private Collider collider;

    protected override void Start()
    {
        if (itemDataSO)
        {
            ItemId = itemDataSO.ItemID;
            ItemRare = itemDataSO.Tier;
            Sprite = itemDataSO.ItemSprite;
            if(itemDataSO)
                ItemType = itemDataSO.itemType;
        }
    }

    public void Visualize(bool value)
    {
        gameObject.GetComponent<Renderer>().enabled = value;
        gameObject.GetComponent<Rigidbody>().isKinematic = value;
        collider.enabled = false;
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

    public override Sprite GetSprite()
    {
        return Sprite;
    }
}

public enum ItemType
{
    Equip,Loot
}

public enum EquipType
{
    Backpack, none
}
